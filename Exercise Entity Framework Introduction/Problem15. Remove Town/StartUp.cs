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
            string result = RemoveTown(dbContext);
            Console.WriteLine(result);
        }

        public static string RemoveTown(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();
            Town townToDelete = context
                .Towns
                .First(t => t.Name == "Seattle");

            Address[] addressesToDelete = context
                .Addresses
                .Where(a => a.Town.Name == "Seattle")
                .ToArray();
            Employee[] employeesToNullAddress = context
                .Employees
                .Where(e => e.Address.Town.Name == "Seattle")
                .ToArray();
            foreach (var emp in employeesToNullAddress)
            {
                emp.AddressId = null;
            }
            int removedAddresses = addressesToDelete.Count();
            context.Addresses.RemoveRange(addressesToDelete);
            context.Towns.Remove(townToDelete);
            context.SaveChanges();
            output.AppendLine($"{removedAddresses} addresses in Seattle were deleted");
            return output.ToString().TrimEnd();
        }
    }
}
