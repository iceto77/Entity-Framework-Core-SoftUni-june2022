namespace Footballers.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            ImportCoachDto[] coachDtos = DeserializeXml<ImportCoachDto[]>(xmlString, "Coaches");
            ICollection<Coach> coaches = new List<Coach>();
            foreach (ImportCoachDto cDto in coachDtos)
            {
                if (!IsValid(cDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Coach coach = new Coach()
                {
                    Name = cDto.Name,
                    Nationality = cDto.Nationality
                };
                foreach (ImportFootballerDto fDto in cDto.Footballers)
                {
                    if (!IsValid(fDto))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    bool isValidContractStartDate = DateTime.TryParseExact(fDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validContractStartDate);
                    if (!isValidContractStartDate)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    bool isValidContractEndDate = DateTime.TryParseExact(fDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validContractEndDate);
                    if (!isValidContractEndDate)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (validContractEndDate < validContractStartDate)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    bool isValidBestSkillType = Enum.IsDefined(typeof(BestSkillType), fDto.BestSkillType);
                    if (!isValidBestSkillType)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    bool isValidPositionType = Enum.IsDefined(typeof(PositionType), fDto.PositionType);
                    if (!isValidPositionType)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer footballer = new Footballer()
                    {
                        Name = fDto.Name,
                        ContractStartDate = validContractStartDate,
                        ContractEndDate = validContractEndDate,
                        BestSkillType = (BestSkillType)fDto.BestSkillType,
                        PositionType = (PositionType)fDto.PositionType
                    };
                    coach.Footballers.Add(footballer);
                }
                coaches.Add(coach);
                output.AppendLine(String.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }

            context.Coaches.AddRange(coaches);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            ImportTeamDto[] teamDtos = JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);
            ICollection<Team> teams = new List<Team>();
            foreach (ImportTeamDto tDto in teamDtos)
            {
                if (!IsValid(tDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                if (tDto.Trophies <= 0)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Team team = Mapper.Map<Team>(tDto);
                int counter = 0;                
                foreach (var fId in tDto.Footballers)
                {
                    bool isValidFootballers = true;
                    counter++;
                    for (int i = 1; i < counter; i++)
                    {
                        if (counter > 1 && tDto.Footballers[i - 1] == fId)
                        {
                            isValidFootballers = false;
                        }
                    }
                    if (!isValidFootballers)
                    {
                        continue;
                    }
                    if (team.TeamsFootballers.Any(tf => tf.FootballerId == fId))
                    {
                        continue;
                    }
                    if (!context.Footballers.Any(f => f.Id == fId))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    TeamFootballer footballer = new TeamFootballer()
                    {
                        Team = team,
                        FootballerId = fId
                    };
                    team.TeamsFootballers.Add(footballer);
                    
                }               
                teams.Add(team);
                output.AppendLine(String.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
            }

            context.Teams.AddRange(teams);
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
