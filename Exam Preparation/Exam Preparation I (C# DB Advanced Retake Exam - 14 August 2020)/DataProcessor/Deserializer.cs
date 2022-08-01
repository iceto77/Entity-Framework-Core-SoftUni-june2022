namespace SoftJail.DataProcessor
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Linq;
    
    using Data;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    
    using Newtonsoft.Json;
    using AutoMapper;
    using System.Globalization;
    using System.Xml.Serialization;
    using System.IO;
    using SoftJail.Data.Models.Enums;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";
        private const string SuccessfullyImportedDepartmentsWithCells = "Imported {0} with {1} cells";
        private const string SuccessfullyImportedPrisonersAndMails = "Imported {0} {1} years old";
        private const string SuccessfullyImportedOfficersAndPrisoners = "Imported {0} ({1} prisoners)";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            ImportDepartmentDto[] departmentDtos = JsonConvert.DeserializeObject<ImportDepartmentDto[]>(jsonString);
            ICollection<Department> departments = new List<Department>();
            foreach (ImportDepartmentDto dDto in departmentDtos)
            {
                if (!IsValid(dDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                if (dDto.Cells.Length == 0)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                if (dDto.Cells.Any(c => !IsValid(c)))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Department department = new Department()
                {
                    Name = dDto.Name
                };
                foreach (var cDto in dDto.Cells)
                {
                    Cell cell = Mapper.Map<Cell>(cDto);
                    department.Cells.Add(cell);
                }
                departments.Add(department);
                output.AppendLine(String.Format(SuccessfullyImportedDepartmentsWithCells, department.Name, department.Cells.Count));
            }
            context.Departments.AddRange(departments);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            ImportPrisonerAndMailsDto[] prisonerDtos = JsonConvert.DeserializeObject<ImportPrisonerAndMailsDto[]>(jsonString);
            ICollection<Prisoner> prisoners = new List<Prisoner>();
            foreach (ImportPrisonerAndMailsDto pDto in prisonerDtos)
            {
                if (!IsValid(pDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                if (pDto.Mails.Any(m => !IsValid(m.Address)))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                bool isValidIncarcerationDate = DateTime.TryParseExact(pDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validIncarcerationDate);
                if (!isValidIncarcerationDate)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                DateTime? validReleaseDate = null;
                if (!String.IsNullOrEmpty(pDto.ReleaseDate))
                {
                    bool isValidReleaseDate = DateTime.TryParseExact(pDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validatedDate);
                    if (!isValidReleaseDate)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    validReleaseDate = validatedDate;
                }
                Prisoner prisoner = new Prisoner()
                {
                    FullName = pDto.FullName,
                    Nickname = pDto.Nickname,
                    Age = pDto.Age,
                    IncarcerationDate = validIncarcerationDate,
                    ReleaseDate = validReleaseDate,
                    Bail = pDto.Bail,
                    CellId = pDto.CellId
                };
                foreach (var mDto in pDto.Mails)
                {
                    Mail mail = Mapper.Map<Mail>(mDto);
                    prisoner.Mails.Add(mail);
                }
                prisoners.Add(prisoner);
                output.AppendLine(String.Format(SuccessfullyImportedPrisonersAndMails, prisoner.FullName, prisoner.Age));
            }
            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            ImportOfficersAndPrisonersDto[] officerDtos = DeserializeXml<ImportOfficersAndPrisonersDto[]>(xmlString, "Officers");
            ICollection<Officer> officers = new List<Officer>();
            foreach (ImportOfficersAndPrisonersDto oDto in officerDtos)
            {
                if (!IsValid(oDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                bool isValidPosition = Enum.TryParse(typeof(Position), oDto.Position, out object positionObj);
                if (!isValidPosition)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                bool isValidWeapon = Enum.TryParse(typeof(Weapon), oDto.Weapon, out object weaponObj);
                if (!isValidWeapon)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Officer officer = new Officer()
                {
                    FullName = oDto.FullName,
                    Salary = oDto.Salary,
                    Position = (Position)positionObj,
                    Weapon = (Weapon)weaponObj,
                    DepartmentId = oDto.DepartmentId
                };
                foreach (var pDto in oDto.Prisoners)
                {
                    OfficerPrisoner op = new OfficerPrisoner()
                    {
                        Officer = officer,
                        PrisonerId = pDto.Id
                    };
                    officer.OfficerPrisoners.Add(op);
                }
                officers.Add(officer);
                output.AppendLine(String.Format(SuccessfullyImportedOfficersAndPrisoners, officer.FullName, officer.OfficerPrisoners.Count));
            }
            context.Officers.AddRange(officers);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
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