namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            StringBuilder output = new StringBuilder();
            ImportProjectDto[] projectDtos = DeserializeXml<ImportProjectDto[]>(xmlString, "Projects");
            ICollection<Project> projects = new List<Project>();
            foreach (ImportProjectDto pDto in projectDtos)
            {
                if (!IsValid(pDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                bool isValidOpenDate = DateTime.TryParseExact(pDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validOpenDate);
                if (!isValidOpenDate)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }            
                bool isValidDueDate = DateTime.TryParseExact(pDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validDueDate);
                if (!String.IsNullOrEmpty(pDto.DueDate) && !isValidDueDate)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                DateTime? EmptyDueDate = null;
                Project project = new Project()
                {
                    Name = pDto.Name,
                    OpenDate = validOpenDate,
                    DueDate = String.IsNullOrEmpty(pDto.DueDate) ? EmptyDueDate : validDueDate 
                };
                foreach (var tDto in pDto.Tasks)
                {
                    if (!IsValid(tDto))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    bool isValidTaskOpenDate = DateTime.TryParseExact(tDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validTaskOpenDate);
                    if (!isValidTaskOpenDate)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    bool isValidTaskDueDate = DateTime.TryParseExact(tDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validTaskDueDate);
                    if (!isValidTaskDueDate)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (validOpenDate > validTaskOpenDate)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (!String.IsNullOrEmpty(pDto.DueDate) && validTaskDueDate > validDueDate)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    bool isValidExecutionType = Enum.TryParse(typeof(ExecutionType), tDto.ExecutionType, out object executionTypeObj);
                    if (!isValidExecutionType)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    bool isValidLabelType = Enum.TryParse(typeof(LabelType), tDto.LabelType, out object labelTypeObj);
                    if (!isValidLabelType)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    Task task = new Task()
                    {
                        Name = tDto.Name,
                        OpenDate = validTaskOpenDate,
                        DueDate = validTaskDueDate,
                        ExecutionType = (ExecutionType)executionTypeObj,
                        LabelType = (LabelType)labelTypeObj
                    };
                    project.Tasks.Add(task);
                }
                projects.Add(project);
                output.AppendLine(String.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));

            }
            context.Projects.AddRange(projects);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            ImportEmployeeDto[] employeeDtos = JsonConvert.DeserializeObject<ImportEmployeeDto[]>(jsonString);
            ICollection<Employee> employees = new List<Employee>();
            foreach (ImportEmployeeDto eDto in employeeDtos)
            {
                if (!IsValid(eDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Employee employee = Mapper.Map<Employee>(eDto);
                foreach (var taskId in eDto.Tasks)
                {
                    var isTaskExsists = context.Tasks.Any(t => t.Id == taskId);
                    if (!isTaskExsists)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    var isNotUniqueTask = employee.EmployeesTasks.Any(e => e.TaskId == taskId);
                    if (isNotUniqueTask)
                    {
                        continue;
                    }
                    employee.EmployeesTasks.Add(new EmployeeTask
                    {
                        Employee = employee,
                        TaskId = taskId
                    });
                }
                employees.Add(employee);
                output.AppendLine(String.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
            }
            context.Employees.AddRange(employees);
            context.SaveChanges();
            return output.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static T DeserializeXml<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            using StringReader reader = new StringReader(inputXml);
            T dtos = (T)xmlSerializer.Deserialize(reader);

            return dtos;
        }
    }
}