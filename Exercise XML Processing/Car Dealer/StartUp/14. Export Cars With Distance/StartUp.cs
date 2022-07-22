
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

            string output = GetCarsWithDistance(dbContext);
            Console.WriteLine(output);
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            ExportCarsWithDistanceDto[] carDtos = context
                .Cars
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new ExportCarsWithDistanceDto() 
                { 
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                //.ProjectTo<ExportCarsWithDistanceDto>()
                .ToArray();

            return Serialize<ExportCarsWithDistanceDto[]>(carDtos, "cars");
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