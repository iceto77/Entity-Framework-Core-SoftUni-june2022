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
            string result = GetDepartmentsWithMoreThan5Employees(dbContext);
            Console.WriteLine(result);
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();
            var moreDepartments = context
                .Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees
                    .Select(e => new
                    {
                        EmplFirstName = e.FirstName,
                        EmplLastName = e.LastName,
                        EmplJobTitle = e.JobTitle
                    })
                    .OrderBy(e => e.EmplFirstName)
                    .ThenBy(e => e.EmplLastName)
                    .ToArray()
                })
                .ToArray();
            foreach (var dep in moreDepartments)
            {
                output.AppendLine($"{dep.DepartmentName} - {dep.ManagerFirstName} {dep.ManagerLastName}");
                foreach (var emp in dep.Employees)
                {
                    output.AppendLine($"{emp.EmplFirstName} {emp.EmplLastName} - {emp.EmplJobTitle}");
                }
            }
            return output.ToString().TrimEnd();
        }
    }
}
