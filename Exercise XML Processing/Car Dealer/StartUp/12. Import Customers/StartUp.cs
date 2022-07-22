
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

            string inputXml = File.ReadAllText("../../../Datasets/customers.xml");
            string output = ImportCustomers(dbContext, inputXml);
            Console.WriteLine(output);
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            ImportCustomerDto[] customerDtos = Deserialize<ImportCustomerDto[]>(inputXml, "Customers");
            ICollection<Customer> customers = new List<Customer>();
            foreach (ImportCustomerDto cDto in customerDtos)
            {
                var mapper = config.CreateMapper();
                Customer customer = mapper.Map<Customer>(cDto);
                customers.Add(customer);
            }
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count}";
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