namespace ProductShop
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;

    using ProductShop.Data;
    using ProductShop.Models;
    using ProductShop.DTOs.User;
    using ProductShop.DTOs.Product;

    using AutoMapper;
    using Newtonsoft.Json;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(ProductShopProfile)));            
            ProductShopContext dbContext = new ProductShopContext();
            string inputJson = File.ReadAllText("../../../Datasets/products.json");
            string output = ImportProducts(dbContext, inputJson);
            Console.WriteLine(output);
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            ImportProductDto[] productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);
            ICollection<Product> products = new List<Product>();
            foreach (ImportProductDto pDto in productDtos)
            {
                if (!IsValid(pDto))
                {
                    continue;
                }
                Product product = Mapper.Map<Product>(pDto);
                products.Add(product);
            }
            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Count}";
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