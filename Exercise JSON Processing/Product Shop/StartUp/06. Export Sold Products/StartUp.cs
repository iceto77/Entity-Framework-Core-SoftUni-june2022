namespace ProductShop
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;

    using ProductShop.Data;
    using ProductShop.Models;
    using ProductShop.DTOs.User;
    using ProductShop.DTOs.Product;
    using ProductShop.DTOs.Category;

    using AutoMapper;
    using Newtonsoft.Json;
    using ProductShop.DTOs.CategoryProduct;
    using AutoMapper.QueryableExtensions;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(ProductShopProfile)));            
            ProductShopContext dbContext = new ProductShopContext();
            string output = GetSoldProducts(dbContext);
            File.WriteAllText("../../../Results/products-in-range.json", output);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            ExportUsersWithSoldProductsDto[] users = context
                    .Users
                    .Where(u => u.ProductsSold.Any(ps => ps.BuyerId.HasValue))
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .ProjectTo<ExportUsersWithSoldProductsDto>() 
                    .ToArray();
            string json = JsonConvert.SerializeObject(users, Formatting.Indented); 
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