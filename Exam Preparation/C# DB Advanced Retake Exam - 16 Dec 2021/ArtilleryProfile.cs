namespace Artillery
{
    using Artillery.Data.Models;
    using Artillery.DataProcessor.ExportDto;
    using Artillery.DataProcessor.ImportDto;

    using AutoMapper;
    class ArtilleryProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public ArtilleryProfile()
        {
            this.CreateMap<ImportCountryDto, Country>();
            this.CreateMap<ImportManufacturerDto, Manufacturer>();
            this.CreateMap<ImportShellDto, Shell>();
            this.CreateMap<ImportGunDto, Gun>();

            //this.CreateMap<Gun, ExportGunDto>();
           // this.CreateMap<Shell, ExportShellsDto>();
        }
    }
}