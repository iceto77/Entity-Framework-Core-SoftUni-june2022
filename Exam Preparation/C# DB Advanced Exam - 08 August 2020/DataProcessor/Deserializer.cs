namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Globalization;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
    using System.Xml.Serialization;
    
	using VaporStore.Data;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dto.Import;
    using VaporStore.Data.Models.Enums;
    
	using Newtonsoft.Json;
    using AutoMapper;

    public static class Deserializer
	{
		private const string ErrorMessage = "Invalid Data";
		private const string SuccessfullyImportedGames = "Added {0} ({1}) with {2} tags";
		private const string SuccessfullyImportedUsers = "Imported {0} with {1} cards";
		private const string SuccessfullyImportedPurchases = "Imported {0} for {1}";
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			StringBuilder output = new StringBuilder();
			ImportGameDto[] gameDtos = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);
			ICollection<Game> games = new List<Game>();
            foreach (ImportGameDto gDto in gameDtos)
            {
				if (!IsValid(gDto))
				{
					output.AppendLine(ErrorMessage);
					continue;
				}
                if (gDto.Name == null || gDto.ReleaseDate == null || gDto.Developer == null || gDto.Genre == null || gDto.Tags.Length == 0 || gDto.Tags == null)
                {
					output.AppendLine(ErrorMessage);
					continue;
				}
				bool isValidReleaseDate = DateTime.TryParseExact(gDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validReleaseDate);
				if (!isValidReleaseDate)
				{
					output.AppendLine(ErrorMessage);
					continue;
				}
                if (gDto.Tags.Length == 0)
                {
					output.AppendLine(ErrorMessage);
					continue;
				}
                if (!context.Developers.Any(d => d.Name == gDto.Developer))
                {
					Developer developer = new Developer()
					{
						Name = gDto.Developer
					};
					context.Developers.Add(developer);
                }
                if (!context.Genres.Any(g => g.Name == gDto.Genre))
                {
					Genre genre = new Genre()
					{
						Name = gDto.Genre
					};
					context.Genres.Add(genre);
                }
				context.SaveChanges();
				Game game = new Game()
				{
					Name = gDto.Name,
					Price = gDto.Price,
					ReleaseDate = validReleaseDate,
					DeveloperId = context.Developers.First(d => d.Name == gDto.Developer).Id,
					GenreId = context.Genres.First(g => g.Name == gDto.Genre).Id
				};
                foreach (var tagName in gDto.Tags)
                {
                    if (!context.Tags.Any(t => t.Name == tagName))
                    {
						Tag currTag = new Tag()
						{
							Name = tagName
						};
						context.Tags.Add(currTag);
                    }
					context.SaveChanges();
					GameTag gameTag = new GameTag()
					{
						Game = game,
						TagId = context.Tags.First(t => t.Name == tagName).Id
					};
					game.GameTags.Add(gameTag);
				}
				games.Add(game);
				output.AppendLine(String.Format(SuccessfullyImportedGames, game.Name, gDto.Genre, gDto.Tags.Length));
			}
			context.Games.AddRange(games);
			context.SaveChanges();
			return output.ToString().TrimEnd();
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			StringBuilder output = new StringBuilder();
			ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString);
			ICollection<User> users = new List<User>();
            foreach (ImportUserDto uDto in userDtos)
            {
				if (!IsValid(uDto))
				{
					output.AppendLine(ErrorMessage);
					continue;
				}
				bool isValidCard = true;
                foreach (var cardDto in uDto.Cards)
                {
					if (!IsValid(cardDto))
					{
						isValidCard = false;
						continue;
					}
					bool isValidCardType = Enum.TryParse(typeof(CardType), cardDto.Type, out object positionObj);
					if (!isValidCardType)
					{
						isValidCard = false;
						continue;
					}
				}
                if (!isValidCard)
                {
					output.AppendLine(ErrorMessage);
					continue;
				}
				User user = Mapper.Map<User>(uDto);
                foreach (var cardDto in uDto.Cards)
                {
					bool isValidCardType = Enum.TryParse(typeof(CardType), cardDto.Type, out object cardTypeObj);
					Card card = new Card()
					{
						Number = cardDto.Number,
						Cvc = cardDto.CVC,
						Type = (CardType)cardTypeObj
					};
					user.Cards.Add(card);
                }
                users.Add(user);
                output.AppendLine(String.Format(SuccessfullyImportedUsers, user.Username, user.Cards.Count));
            }
			context.Users.AddRange(users);
			context.SaveChanges();
			return output.ToString().TrimEnd();
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
			StringBuilder output = new StringBuilder();
			ImportPurchaseDto[] purchaseDtos = DeserializeXml<ImportPurchaseDto[]>(xmlString, "Purchases");
			ICollection<Purchase> purchases = new List<Purchase>();
			foreach (ImportPurchaseDto pDto in purchaseDtos)
            {
				if (!IsValid(pDto))
				{
					output.AppendLine(ErrorMessage);
					continue;
				}
				bool isValidPurchaseType = Enum.TryParse(typeof(PurchaseType), pDto.Type, out object purchaseTypeObj);
				if (!isValidPurchaseType)
				{
					output.AppendLine(ErrorMessage);
					continue;
				}
				bool isValidDate = DateTime.TryParseExact(pDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validDate);
				if (!isValidDate)
				{
					output.AppendLine(ErrorMessage);
					continue;
				}
				Purchase purchase = new Purchase()
				{					
					Type = (PurchaseType)purchaseTypeObj,
					ProductKey = pDto.ProductKey,
					Date = validDate
				};
				purchase.Game = context.Games.First(g => g.Name == pDto.GameName);
				purchase.Card = context.Cards.First(c => c.Number == pDto.Card);
				purchases.Add(purchase);
				string gameName = pDto.GameName;
				string username = context.Users.First(u => u.Id == purchase.Card.UserId).Username;
				output.AppendLine(String.Format(SuccessfullyImportedPurchases, gameName, username));
			}
			context.Purchases.AddRange(purchases);
			context.SaveChanges();
			return output.ToString().TrimEnd();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}

		private static T DeserializeXml<T>(string inputXml, string rootName)
		{
			XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

			using StringReader reader = new StringReader(inputXml);
			T dtos = (T)xmlSerializer.Deserialize(reader);

			return dtos;
		}
	}
}