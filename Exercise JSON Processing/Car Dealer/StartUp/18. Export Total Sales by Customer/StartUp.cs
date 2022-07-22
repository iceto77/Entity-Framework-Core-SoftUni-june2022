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

            string output = GetTotalSalesByCustomer(dbContext);
            File.WriteAllText("../../../Results/customers-total-sales.json", output);
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            ExportCustomerWithSalesDto[] customersWithSales = context
                .Customers
                .Where(c => c.Sales.Count > 0)
                .OrderByDescending(c => c.Sales.SelectMany(s => s.Car.PartCars).Sum(pc => pc.Part.Price))
                .ThenByDescending(c => c.Sales.Count)
                .ProjectTo<ExportCustomerWithSalesDto>()
                .ToArray();
            string json = JsonConvert.SerializeObject(customersWithSales, Formatting.Indented);

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