namespace Theatre
{
    using AutoMapper;
    using System;
    using System.Globalization;
    using System.Linq;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ExportDto;
    using Theatre.DataProcessor.ImportDto;

    class TheatreProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public TheatreProfile()
        {
            this.CreateMap<ImportPlayDto, Play>()
                .ForMember(d => d.Duration, mo => mo.MapFrom(s => TimeSpan.ParseExact(s.Duration, "c", CultureInfo.InvariantCulture)))
                .ForMember(d => d.Genre, mo => mo.MapFrom(s => (Genre)Enum.Parse(typeof(Genre), s.Genre)));
            this.CreateMap<ImportCastDto, Cast>()
                .ForMember(d => d.IsMainCharacter, mo => mo.MapFrom(s => bool.Parse(s.IsMainCharacter)))
                .ForMember(d => d.PlayId, mo => mo.MapFrom(s => s.PlayId));
            this.CreateMap<ImportProjectionDto, Theatre>()
                .ForMember(d => d.Tickets, mo => mo.Ignore());
            this.CreateMap<ImportTicketDto, Ticket>()
                .ForMember(d => d.PlayId, mo => mo.MapFrom(s => s.PlayId));

        }
    }
}
