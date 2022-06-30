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
            string result = GetEmployeesInPeriod(dbContext);
            Console.WriteLine(result);
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();
            var employeesWithProjects = context
                .Employees
                .Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    AllProjects = e.EmployeesProjects
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name,
                            StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt"),
                            EndDate = ep.Project.EndDate.HasValue ?
                                      ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt")
                                    : $"not finished"

                        })
                        .ToArray()
                })
                .ToArray();
           
            foreach (var emp in employeesWithProjects)
            {
                output.AppendLine($"{emp.FirstName} {emp.LastName} - Manager: {emp.ManagerFirstName} {emp.ManagerLastName}");
                foreach (var prj in emp.AllProjects)
                {
                    output.AppendLine($"--{prj.ProjectName} - {prj.StartDate} - {prj.EndDate}");
                }
            }
            return output.ToString().TrimEnd();
        }
    }
}
