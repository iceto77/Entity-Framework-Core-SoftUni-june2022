namespace CarDealer
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    using CarDealer.Models;
    using CarDealer.DTO;

    using AutoMapper;
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSupplierDto, Supplier>();
            this.CreateMap<ImportPartDto, Part>();
            this.CreateMap<ImportCarDto, Car>();
            this.CreateMap<ImportCustomerDto, Customer>();
            this.CreateMap<ImportSaleDto, Sale>();

            this.CreateMap<Customer, ExportOrderedCustomersDto>()
                .ForMember(d => d.BirthDate, mo => mo.MapFrom(s => s.BirthDate.ToString("dd/MM/yyyy")));
            this.CreateMap<Car, ExportToyotaCarDto>();
            this.CreateMap<Supplier, ExportLocalSupplierInfoDto>()
                .ForMember(d => d.PartsCount, mo => mo.MapFrom(s => s.Parts.Count));

            this.CreateMap<Car, ExportCarAndPartsDto>()
                .ForMember(d => d.Car, mo => mo.MapFrom(s => new ExportCarInfoDto
                {
                    Make = s.Make,
                    Model = s.Model,
                    TravelledDistance = s.TravelledDistance
                }))
                .ForMember(d => d.Parts, mo => mo.MapFrom(s => s.PartCars
                    .Select(pc => new ExportPartInfoDto
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price.ToString("f2")
                    })
                    .ToArray()));

            this.CreateMap<Customer, ExportCustomerWithSalesDto>()
                .ForMember(d => d.FullName, mo => mo.MapFrom(s => s.Name))
                .ForMember(d => d.BoughtCars, mo => mo.MapFrom(s => s.Sales.Count))
                .ForMember(d => d.SpentMoney, mo => mo.MapFrom(s => s.Sales.SelectMany(s => s.Car.PartCars).Sum(pc => pc.Part.Price)));

            this.CreateMap<Sale, ExportSalesWithDiscountDto>()
                .ForMember(d => d.Car, mo => mo.MapFrom(s => new ExportCarInfoDto 
                    { 
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    }))
                .ForMember(d => d.CustomerName, mo => mo.MapFrom(s => s.Customer.Name))
                .ForMember(d => d.Discount, mo => mo.MapFrom(s => s.Discount.ToString("F2")))
                .ForMember(d => d.Price, mo => mo.MapFrom(s => s.Car.PartCars.Sum(pc => pc.Part.Price).ToString("F2")))
                .ForMember(d => d.PriceWithDiscount, mo => mo.MapFrom(s => (s.Car.PartCars.Sum(pc => pc.Part.Price) 
                                                        - (s.Car.PartCars.Sum(pc => pc.Part.Price) * (s.Discount / 100))).ToString("F2")));
        }
    }
}
