namespace ProductShop
{
    using System;
    using System.IO;

    using ProductShop.Data;
    using ProductShop.DTOs.User;

    using AutoMapper;
    using Newtonsoft.Json;
    using ProductShop.Models;
    using System.Collections.Generic;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(ProductShopProfile)));            
            ProductShopContext dbContext = new ProductShopContext();

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();
            //Console.WriteLine($"Database copy was created!");

            string inputJson = File.ReadAllText("../../../Datasets/users.json");
	    string output =  ImportUsers(dbContext, inputJson);
            Console.WriteLine(output);
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);
            ICollection<User> users = new List<User>();
            foreach (ImportUserDto uDto in userDtos)
            {
                User user = Mapper.Map<User>(uDto);
                users.Add(user);
            }
            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count}";
        }
    }
}