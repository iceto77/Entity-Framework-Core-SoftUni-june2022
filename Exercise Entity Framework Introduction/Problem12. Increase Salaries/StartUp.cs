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
            string result = IncreaseSalaries(dbContext);
            Console.WriteLine(result);
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();
           
            var employeesWithNewSalary = context
                .Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" || e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();
            foreach (var emp in employeesWithNewSalary)
            {
                emp.Salary *= (decimal)1.12;
            }
            context.SaveChanges();
            foreach (var emp in employeesWithNewSalary)
            {
                output.AppendLine($"{emp.FirstName} {emp.LastName} (${emp.Salary:f2})");
            }
            return output.ToString().TrimEnd();
        }
    }
}
