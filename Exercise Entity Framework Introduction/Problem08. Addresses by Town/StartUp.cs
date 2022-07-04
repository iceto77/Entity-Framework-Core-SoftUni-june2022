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
            string result = GetAddressesByTown(dbContext);
            Console.WriteLine(result);
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();


            var addressInfo = context
                .Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .Take(10)
                .Select(a => new
                     {
                        AddressText = a.AddressText,
                        TownName = a.Town.Name,
                        EmployeeCount = a.Employees.Count
                     })
                .ToList();

            foreach (var adr in addressInfo)
            {
                output.AppendLine($"{adr.AddressText}, {adr.TownName} - {adr.EmployeeCount} employees");
            }
            return output.ToString().TrimEnd();
        }
    }
}
