namespace SoftJail
{
    using System.Linq;

    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;

    using AutoMapper;
    using SoftJail.DataProcessor.ExportDto;
    using System;

    public class SoftJailProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public SoftJailProfile()
        {
            this.CreateMap<ImportCellInfoDto, Cell>();
            this.CreateMap<ImportMailInfoDto, Mail>();
            this.CreateMap<Mail, ExportMessageDto>()
                .ForMember(d => d.Description, mo => mo.MapFrom(s => String.Join("", s.Description.Reverse())));
            this.CreateMap<Prisoner, ExportInboxForPrisonerDto>()
                .ForMember(d => d.IncarcerationDate, mo => mo.MapFrom(s => s.IncarcerationDate.ToString("yyyy-MM-dd")))
                .ForMember(d => d.EncryptedMessages, mo => mo.MapFrom(s => s.Mails));

        }
    }
}
