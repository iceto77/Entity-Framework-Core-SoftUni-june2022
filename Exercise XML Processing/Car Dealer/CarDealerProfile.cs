namespace CarDealer
{
    using System;
    using System.Linq;
    using System.Globalization;

    using CarDealer.Models;
    using CarDealer.Dtos.Import;

    using AutoMapper;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSupplierDto, Supplier>();
            this.CreateMap<ImportPartDto, Part>();
            this.CreateMap<ImportCarDto, Car>();
            this.CreateMap<ImportCustomerDto, Customer>()
                .ForMember(d => d.BirthDate, mo => mo.MapFrom(s => DateTime.Parse(s.BirthDate, CultureInfo.InvariantCulture)));
            this.CreateMap<ImportSaleDto, Sale>();

        }
    }
}
