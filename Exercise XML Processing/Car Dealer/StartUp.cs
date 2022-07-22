
namespace CarDealer
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using CarDealer.Models;
    using CarDealer.Data;
    using CarDealer.Dtos.Import;
    using CarDealer.Dtos.Export;

    public class StartUp
    {      
        public static void Main(string[] args)
        {            
            CarDealerContext dbContext = new CarDealerContext();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            Console.WriteLine($"Database copy was created!");

	// това е само за създаване на базата данни. ЗА всяка задача замени от папката StartUp със съответния файл

        }        
    }
}