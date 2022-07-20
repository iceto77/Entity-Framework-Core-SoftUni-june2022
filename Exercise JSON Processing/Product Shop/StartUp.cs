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
        private static string filePath;
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(ProductShopProfile)));            
            ProductShopContext dbContext = new ProductShopContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
	    Console.WriteLine($"Database copy was created!");

            // това е само за създаване на базата данни. ЗА всяка задача замени от папката StartUp със съответния файл
        }
    }
}