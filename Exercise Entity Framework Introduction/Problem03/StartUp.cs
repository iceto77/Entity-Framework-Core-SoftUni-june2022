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
            string result = GetEmployeesFullInformation(dbContext);
            Console.WriteLine(result);
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();
            Employee[] allEmployees = context
                .Employees
                .OrderBy(e => e.EmployeeId)
                .ToArray();

            foreach (Employee emp in allEmployees)
            {
                output.AppendLine($"{emp.FirstName} {emp.LastName} {emp.MiddleName} {emp.JobTitle} {emp.Salary:f2}");
            }
            return output.ToString().TrimEnd();
        }
    }
}
