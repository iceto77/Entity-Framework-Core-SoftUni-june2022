
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

            string inputXml = File.ReadAllText("../../../Datasets/parts.xml");
            string output = ImportParts(dbContext, inputXml);
            Console.WriteLine(output);
        }

	public static string ImportParts(CarDealerContext context, string inputXml)
        {
            ImportPartDto[] partDtos = Deserialize<ImportPartDto[]>(inputXml, "Parts");
            ICollection<Part> parts = new List<Part>();
            foreach (ImportPartDto pDto in partDtos)
            {
                bool supplierExsists = context.Suppliers.Any(s => s.Id == pDto.SupplierId);
                if (!supplierExsists)
                {
                    continue;
                }
                var mapper = config.CreateMapper();
                Part part = mapper.Map<Part>(pDto);
                parts.Add(part);
            }
            context.Parts.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count}";
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