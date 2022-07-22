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

            string inputJson = File.ReadAllText("../../../Datasets/parts.json");
            string output = ImportParts(dbContext, inputJson);
            Console.WriteLine(output);
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            ImportPartDto[] partDtos = JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson);
            ICollection<Part> parts = new List<Part>();
            foreach (ImportPartDto pDto in partDtos)
            {
                if (!IsValid(pDto))
                {
                    continue;
                }
                if (!context.Suppliers.Any(s => s.Id == pDto.SupplierId))
                {
                    continue;
                }
                Part part = Mapper.Map<Part>(pDto);
                parts.Add(part);
            }
            context.Parts.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count}.";
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