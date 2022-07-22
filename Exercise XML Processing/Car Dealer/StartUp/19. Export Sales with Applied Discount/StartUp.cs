
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

            string output = GetSalesWithAppliedDiscount(dbContext);
            Console.WriteLine(output);
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            ExportSalesWithDiscountDto[] saleDtos = context
                .Sales
                .Select(s => new ExportSalesWithDiscountDto()
                {
                    Car =  new ExportSaleCarDto() 
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    Discount = (double)s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = (double)s.Car.PartCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = (double)(s.Car.PartCars.Sum(pc => pc.Part.Price) - s.Car.PartCars.Sum(pc => pc.Part.Price)*( s.Discount / 100))
                })
                .ToArray();

            return Serialize<ExportSalesWithDiscountDto[]>(saleDtos, "sales");
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