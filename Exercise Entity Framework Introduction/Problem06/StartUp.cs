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
            string result = AddNewAddressToEmployee(dbContext);
            Console.WriteLine(result);
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();
            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            context.Addresses.Add(newAddress);

            Employee nakov = context
                .Employees
                .First(e => e.LastName == "Nakov");
            nakov.Address = newAddress;
            context.SaveChanges();

            string[] addressText = context
                .Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address.AddressText)
                .ToArray();
           
            foreach (var ad in addressText)
            {
                output.AppendLine(ad);
            }
            return output.ToString().TrimEnd();
        }
    }
}
