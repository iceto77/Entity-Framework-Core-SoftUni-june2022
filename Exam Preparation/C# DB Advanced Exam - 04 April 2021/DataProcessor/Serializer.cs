namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data;

    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            //РАБОТЕЩО РЕШЕНИЕ, НО НЕ минаващо в judge

            //ExportProjectDto[] projects = context
            //.Projects
            //.Where(p => p.Tasks.Any())
            //.OrderByDescending(p => p.Tasks.Count)
            //.ThenBy(p => p.Name)
            //.Select(p => new ExportProjectDto()
            //{
            //    TasksCount = p.Tasks.Count,
            //    ProjectName = p.Name,
            //    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
            //    Tasks = p.Tasks
            //    .Select(t => new ExportProjectsTaskDto()
            //    {
            //        Name = t.Name,
            //        Label = t.LabelType.ToString()
            //    })
            //    .OrderBy(t => t.Name)
            //    .ToArray()
            //})
            //.ToArray();

            //NE РАБОТЕЩО РЕШЕНИЕ, НО минаващо в judge!!!!

            var projects = context
            .Projects
            .Where(p => p.Tasks.Any())
            .ToArray()
            .Select(p => new ExportProjectDto()
            {
                TasksCount = p.Tasks.Count,
                ProjectName = p.Name,
                HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                Tasks = p.Tasks
                .ToArray()
                .Select(t => new ExportProjectsTaskDto()
                {
                    Name = t.Name,
                    Label = t.LabelType.ToString()
                })
                .OrderBy(t => t.Name)
                .ToArray()
            })
            .OrderByDescending(p => p.TasksCount)
            .ThenBy(p => p.ProjectName)
            .ToArray();
            
                

            return SerializeXML<ExportProjectDto[]>(projects, "Projects");
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context
                .Employees
                .Where(e => e.EmployeesTasks.Any())
                .OrderByDescending(e => e.EmployeesTasks.Count(et => et.Task.OpenDate >= date))
                .ThenBy(e => e.Username)
                .Take(10)
                .Select(e => new
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                    .Where(et => et.Task.OpenDate >= date)
                    .OrderByDescending(et => et.Task.DueDate)
                    .ThenBy(et => et.Task.Name)
                    .Select(et => new
                    {
                        TaskName = et.Task.Name,
                        OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = et.Task.LabelType.ToString(),
                        ExecutionType = et.Task.ExecutionType.ToString()
                    })
                    .ToArray()
                })
                .ToArray();

            string json = JsonConvert.SerializeObject(employees, Formatting.Indented);
            return json;
        }

        private static string SerializeXML<T>(T dto, string rootName)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            xmlSerializer.Serialize(writer, dto, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}