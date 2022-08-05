namespace VaporStore.DataProcessor
{
	using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
            var geners = context
                .Genres
                .Where(gn => genreNames.Contains(gn.Name))
                .Select(gn => new
                {
                    Id = gn.Id,
                    Genre = gn.Name,
                    Games = gn.Games
                    .Where(gm => gm.Purchases.Count > 0)
                    .OrderByDescending(gm => gm.Purchases.Count)
                    .ThenBy(gm => gm.Id)
                    .Select(gm => new 
                    {
                        Id = gm.Id,
                        Title = gm.Name,
                        Developer = gm.Developer.Name,
                        Tags = String.Join(", ", gm.GameTags.Select(gt => gt.Tag.Name)),
                        Players = gm.Purchases.Count
                    })
                    .OrderByDescending(gm => gm.Players)
                    .ThenBy(gm => gm.Id)
                    .ToArray(),
                    TotalPlayers = gn.Games.Select(gm => gm.Purchases).Count()
                })
                .ToArray()
                .OrderByDescending(g => g.TotalPlayers)
                .ThenBy(g => g.Id)
                .ToArray();
            string json = JsonConvert.SerializeObject(geners, Formatting.Indented);
            return json;
        }

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
            ExportUserDto[] users = context
                .Users
                .Where(u => u.Cards.Any(c => c.Purchases.Any()))
                .Select(u => new ExportUserDto()
                {
                    Username = u.Username,
                    Purchases = u.Cards.Select(c => new ExportUsersPurchaseDto() 
                    { 
                        Card = c.Number,
                        Cvc = c.Cvc,
                        Date = u.Cards.SelectMany(c => c.Purchases.)
                    }).ToArray()
                })
                .ToArray();
               

            return SerializeXML<ExportUserDto[]>(users, "Users");
        }

        private static string SerializeXML<T>(T dto, string rootName)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            xmlSerializer.Serialize(writer, dto, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}