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

            string inputJson = File.ReadAllText("../../../Datasets/customers.json");
            string output = ImportCustomers(dbContext, inputJson);
            Console.WriteLine(output);
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            ImportCustomerDto[] customerDtos = JsonConvert.DeserializeObject<ImportCustomerDto[]>(inputJson);
            ICollection<Customer> customers = new List<Customer>();
            foreach (ImportCustomerDto cDto in customerDtos)
            {
                if (!IsValid(cDto))
                {
                    continue;
                }
                Customer customer = Mapper.Map<Customer>(cDto);
                customers.Add(customer);
            }
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count}.";
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