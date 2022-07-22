
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

            string inputXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            string output = ImportSuppliers(dbContext, inputXml);
            Console.WriteLine(output);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            ImportSupplierDto[] supplierDtos = Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers");

            	//решение с menual maping
            
            //Supplier[] suppliers = supplierDtos
            //.Select(dto => new Supplier()
            //{
            //    Name = dto.Name,
            //    IsImporter = dto.IsImporter
            //})
            //.ToArray();

            //context.Suppliers.AddRange(suppliers);
            //context.SaveChanges();
            //return $"Successfully imported {suppliers.Length}";

		//решение с automaping

            ICollection<Supplier> suppliers = new List<Supplier>();
            foreach (ImportSupplierDto sDto in supplierDtos)
            {
                var mapper = config.CreateMapper();
                Supplier supplier = mapper.Map<Supplier>(sDto);
                suppliers.Add(supplier);
            }
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count}";
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