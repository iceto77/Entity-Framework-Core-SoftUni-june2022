
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

    public class StartUp
    {      
        public static void Main(string[] args)
        {            
            CarDealerContext dbContext = new CarDealerContext();

            string output = GetTotalSalesByCustomer(dbContext);
            Console.WriteLine(output);
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            ExportCustomerTotalSalesDto[] customerDtos = context
                .Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new ExportCustomerTotalSalesDto()
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.SelectMany(s => s.Car.PartCars).Sum(pc => pc.Part.Price)
                })
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            return Serialize<ExportCustomerTotalSalesDto[]>(customerDtos, "customers");
        }

    	private static string Serialize<T>(T dto, string rootName)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            xmlSerializer.Serialize(writer, dto, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}