namespace VaporStore
{
	using AutoMapper;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public class VaporStoreProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
		public VaporStoreProfile()
		{
			this.CreateMap<ImportUserDto, User>()
				.ForMember(d => d.Cards, mo => mo.Ignore());
			
		}
	}
}