
namespace CarDealer
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using System.IO;
    using System.Linq;

    using CarDealer.Models;
    using CarDealer.Data;
    using CarDealer.DTO;

    using AutoMapper;

    public class StartUp
    {
        public static MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CarDealerProfile>();
        });        
        public static void Main(string[] args)
        {            
            CarDealerContext dbContext = new CarDealerContext();

            string inputXml = File.ReadAllText("../../../Datasets/sales.xml");
            string output = ImportSales(dbContext, inputXml);
            Console.WriteLine(output);
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            ImportSaleDto[] saleDtos = Deserialize<ImportSaleDto[]>(inputXml, "Sales");
            ICollection<Sale> sales = new List<Sale>();
            foreach (ImportSaleDto sDto in saleDtos)
            {
                bool carExsists = context.Cars.Any(c => c.Id == sDto.CarId);
                if (!carExsists)
                {
                    continue;
                }
                var mapper = config.CreateMapper();
                Sale sale = mapper.Map<Sale>(sDto);
                sales.Add(sale);
            }
            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count}";
        }

    	private static T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            using StringReader reader = new StringReader(inputXml);
            T dtos = (T)xmlSerializer.Deserialize(reader);

            return dtos;
        }
    }
}