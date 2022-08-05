namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.DataProcessor.ImportDto;
    
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data.";
        private const string SuccessfulImportCountry = "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer = "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell = "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun = "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ArtilleryProfile>();
        });

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            ImportCountryDto[] countryDtos = DeserializeXml<ImportCountryDto[]>(xmlString, "Countries");
            ICollection<Country> countries = new List<Country>();
            foreach (ImportCountryDto cDto in countryDtos)
            {
                if (!IsValid(cDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                var mapper = config.CreateMapper();
                Country country = mapper.Map<Country>(cDto);
                countries.Add(country);
                output.AppendLine(String.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }
            context.Countries.AddRange(countries);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            ImportManufacturerDto[] manufacturerDtos = DeserializeXml<ImportManufacturerDto[]>(xmlString, "Manufacturers");
            ICollection<Manufacturer> manufacturers = new List<Manufacturer>();
            foreach (ImportManufacturerDto mDto in manufacturerDtos)
            {
                if (!IsValid(mDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                if (context.Manufacturers.Any(m => m.ManufacturerName == mDto.ManufacturerName))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                if (manufacturers.Any(m => m.ManufacturerName == mDto.ManufacturerName))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                var mapper = config.CreateMapper();
                Manufacturer manufacturer = mapper.Map<Manufacturer>(mDto);
                manufacturers.Add(manufacturer);
                string[] founded = manufacturer.Founded.Split(", ").ToArray();
                int l = founded.Length;
                string outputFounded = $"{founded[l-2]}, {founded[l-1]}";
                output.AppendLine(String.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, outputFounded));
            }
            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            ImportShellDto[] shellDtos = DeserializeXml<ImportShellDto[]>(xmlString, "Shells");
            ICollection<Shell> shells = new List<Shell>();
            foreach (ImportShellDto sDto in shellDtos)
            {
                if (!IsValid(sDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                var mapper = config.CreateMapper();
                Shell shell = mapper.Map<Shell>(sDto);
                shells.Add(shell);
                output.AppendLine(String.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }
            context.Shells.AddRange(shells);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            ImportGunDto[] gunDtos = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);
            ICollection<Gun> guns = new List<Gun>();
            foreach (ImportGunDto gDto in gunDtos)
            {
                if (!IsValid(gDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Gun gun = Mapper.Map<Gun>(gDto);
                foreach (var country in gDto.Countries)
                {
                    CountryGun countryGun = new CountryGun
                    {
                        Gun = gun,
                        CountryId = country.Id
                    };
                    gun.CountriesGuns.Add(countryGun);
                }
                guns.Add(gun);
                output.AppendLine(String.Format(SuccessfulImportGun, gun.GunType, gun.GunWeight, gun.BarrelLength));
            }
            context.Guns.AddRange(guns);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
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
