
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

            string output = GetCarsFromMakeBmw(dbContext);
            Console.WriteLine(output);
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            ExportBmwCarsDto[] carDtos = context
                .Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new ExportBmwCarsDto()
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })               
                .ToArray();

            return Serialize<ExportBmwCarsDto[]>(carDtos, "cars");
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