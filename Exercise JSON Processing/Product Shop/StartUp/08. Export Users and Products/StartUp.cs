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
            string output = GetUsersWithProducts(dbContext);
            File.WriteAllText("../../../Results/products-in-range.json", output);
        }

        //problem08 - с Auto Mapper - не минава в judge
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            //вариянт с automapping
            //ExportUsersWithFullProductInfoDto[] users = context
            //        .Users
            //        .Where(u => u.ProductsSold.Any(ps => ps.BuyerId.HasValue))
            //        .OrderByDescending(u => u.ProductsSold.Count(p => p.BuyerId.HasValue))
            //        .ProjectTo<ExportUsersWithFullProductInfoDto>()
            //        .ToArray();
            //ExportUsersInfoDto serDto = new ExportUsersInfoDto()
            //{
            //    UsersCount = users.Length,
            //    Users = users
            //};

            ExportUsersInfoDto serDto = new ExportUsersInfoDto()
            {
                Users = context
                .Users
                .Where(u => u.ProductsSold.Any(ps => ps.BuyerId.HasValue))
                .OrderByDescending(u => u.ProductsSold.Count(p => p.BuyerId.HasValue))
                .Select(u => new ExportUsersWithFullProductInfoDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProductsInfo = new ExportSoldProductsFullInfoDto()
                    {
                        SoldProducts = u.ProductsSold
                                        .Where(p => p.BuyerId.HasValue)
                                        .Select(p => new ExportSoldProductShortInfoDto()
                                        {
                                            Name = p.Name,
                                            Price = p.Price
                                        })
                                        .ToArray()
                    }
                })
                .ToArray()
            };

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings() 
            {  
                NullValueHandling = NullValueHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(serDto, Formatting.Indented, serializerSettings);

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