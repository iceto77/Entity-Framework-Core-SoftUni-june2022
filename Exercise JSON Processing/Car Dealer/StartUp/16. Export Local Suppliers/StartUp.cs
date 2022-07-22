namespace CarDealer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;

    using CarDealer.Data;
    using CarDealer.Models;
    using CarDealer.DTO;
    
    using AutoMapper;
    using Newtonsoft.Json;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(CarDealerProfile)));
            CarDealerContext dbContext = new CarDealerContext();

            string output = GetLocalSuppliers(dbContext);
            File.WriteAllText("../../../Results/local-suppliers.json", output);
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            ExportLocalSupplierInfoDto[] localSuppliers = context
                .Suppliers
                .Where(s => s.IsImporter == false)
                .ProjectTo<ExportLocalSupplierInfoDto>()
                .ToArray();
            string json = JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);

            return json;
        }

        public static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult);
            return isValid;
        }
    }
}