namespace Theatre.DataProcessor
{
    using AutoMapper;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            ImportPlayDto[] playDtos = DeserializeXml<ImportPlayDto[]>(xmlString, "Plays");
            ICollection<Play> plays = new List<Play>();
            foreach (ImportPlayDto pDto in playDtos)
            {
                if (!IsValid(pDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                TimeSpan duration = TimeSpan.ParseExact(pDto.Duration, "c", CultureInfo.InvariantCulture);
                if (duration.Hours < 1)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                bool isValidGenre = Enum.TryParse(typeof(Genre), pDto.Genre, out object genreObj);
                if (!isValidGenre)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Play play = Mapper.Map<Play>(pDto);
                plays.Add(play);
                output.AppendLine(String.Format(SuccessfulImportPlay, play.Title, play.Genre.ToString(), play.Rating));
            }
            context.Plays.AddRange(plays);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            ImportCastDto[] castDtos = DeserializeXml<ImportCastDto[]>(xmlString, "Casts");
            ICollection<Cast> casts = new List<Cast>();
            foreach (ImportCastDto cDto in castDtos)
            {
                if (!IsValid(cDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                bool isValidBool = bool.TryParse(cDto.IsMainCharacter, out bool validIsMainCharacter);
                if (!isValidBool)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Cast cast = Mapper.Map<Cast>(cDto);
                casts.Add(cast);
                output.AppendLine(String.Format(SuccessfulImportActor, cast.FullName, validIsMainCharacter ? "main" : "lesser"));
            }
            context.Casts.AddRange(casts);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            ImportProjectionDto[] theatreDtos = JsonConvert.DeserializeObject<ImportProjectionDto[]>(jsonString);
            ICollection<Theatre> theatres = new List<Theatre>();
            foreach (ImportProjectionDto tDto in theatreDtos)
            {
                if (!IsValid(tDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Theatre theatre = Mapper.Map<Theatre>(tDto);
                foreach (ImportTicketDto tiDto in tDto.Tickets)
                {
                    if (!IsValid(tiDto))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    Ticket ticket = Mapper.Map<Ticket>(tiDto);
                    theatre.Tickets.Add(ticket);
                }
                theatres.Add(theatre);
                output.AppendLine(String.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
            }
            context.Theatres.AddRange(theatres);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
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
