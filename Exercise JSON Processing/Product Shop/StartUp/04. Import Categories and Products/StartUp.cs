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
    using ProductShop.DTOs.Category;

    using AutoMapper;
    using Newtonsoft.Json;
    using ProductShop.DTOs.CategoryProduct;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(ProductShopProfile)));            
            ProductShopContext dbContext = new ProductShopContext();
            string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");
            string output = ImportCategoryProducts(dbContext, inputJson);
            Console.WriteLine(output);
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            ImportCategoryProductDto[] categoryProductsDtos = JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson);
            ICollection<CategoryProduct> categoryProducts = new List<CategoryProduct>();
            foreach (ImportCategoryProductDto cpDto in categoryProductsDtos)
            {
                if (!IsValid(cpDto))
                {
                    continue;
                }
                CategoryProduct categoryProduct = Mapper.Map<CategoryProduct>(cpDto);
                categoryProducts.Add(categoryProduct);
            }
            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();
            return $"Successfully imported {categoryProducts.Count}";
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