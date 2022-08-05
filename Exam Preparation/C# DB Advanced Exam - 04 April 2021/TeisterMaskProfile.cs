namespace TeisterMask
{
    using AutoMapper;
    using System.Linq;
    using TeisterMask.Data.Models;
    using TeisterMask.DataProcessor.ExportDto;
    using TeisterMask.DataProcessor.ImportDto;

    public class TeisterMaskProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
        public TeisterMaskProfile()
        {
            this.CreateMap<ImportEmployeeDto, Employee>()
                .ForMember(d => d.EmployeesTasks, mo => mo.Ignore());
            this.CreateMap<Task, ExportProjectsTaskDto>()
                .ForMember(d => d.Label, mo => mo.MapFrom(s => s.LabelType.ToString()));
            this.CreateMap<Project, ExportProjectDto>()
                .ForMember(d => d.ProjectName, m => m.MapFrom(s => s.Name))
                .ForMember(d => d.HasEndDate, m => m.MapFrom(s => s.DueDate.HasValue ? "Yes" : "No"))
                .ForMember(d => d.TasksCount, m => m.MapFrom(s => s.Tasks.Count))
                .ForMember(d => d.Tasks, m => m.MapFrom(s => s.Tasks.ToArray().OrderBy(t => t.Name).ToArray()));
        }
    }
}
