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

            string output = GetSalesWithAppliedDiscount(dbContext);
            File.WriteAllText("../../../Results/sales-discounts.json", output);
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            ExportSalesWithDiscountDto[] saleDtos = context
                .Sales
                .ProjectTo<ExportSalesWithDiscountDto>()
                .Take(10)
                .ToArray();
            string json = JsonConvert.SerializeObject(saleDtos, Formatting.Indented);

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