namespace SoftUni
{
    using Data;
    using Models;
    using System;
    using System.Linq;
    using System.Text;
    public class StartUp
    {
        public static void Main(string[] args)
        {
            SoftUniContext dbContext = new SoftUniContext();
            string result = GetEmployee147(dbContext);
            Console.WriteLine(result);
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();

            var employee147Info = context
                .Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle
                });
            foreach (var e in employee147Info)
            {
                output.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
            }
            var employee147Projects = context
                .EmployeesProjects
                .Where(e => e.EmployeeId == 147)
                .Select(ep => new
                {                    
                    Projects = ep.Project.Name
                })   
                .OrderBy(ep => ep.Projects)
                .ToArray();
            
            foreach (var ep in employee147Projects)
            {
                output.AppendLine($"{ep.Projects}");
            }
            return output.ToString().TrimEnd();
        }
    }
}
