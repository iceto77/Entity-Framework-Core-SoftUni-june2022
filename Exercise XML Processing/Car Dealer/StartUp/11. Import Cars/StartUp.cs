
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

            string inputXml = File.ReadAllText("../../../Datasets/cars.xml");
            string output = ImportCars(dbContext, inputXml);
            Console.WriteLine(output);
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            ImportCarDto[] carDtos = Deserialize<ImportCarDto[]>(inputXml, "Cars");
            ICollection<Car> cars = new List<Car>();
            foreach (ImportCarDto cDto in carDtos)
            {                
                var mapper = config.CreateMapper();
                Car car = mapper.Map<Car>(cDto);
                ICollection<PartCar> carParts = new List<PartCar>();
                foreach (var partId in cDto.Parts.Select(p => p.Id).Distinct())
                {
                    bool partExsists = context.Parts.Any(p => p.Id == partId);
                    if (!partExsists)
                    {
                        continue;
                    }
                    carParts.Add(new PartCar()
                    {
                        Car = car,
                        PartId = partId
                    });
                }
                car.PartCars = carParts;
                cars.Add(car);
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count}";
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