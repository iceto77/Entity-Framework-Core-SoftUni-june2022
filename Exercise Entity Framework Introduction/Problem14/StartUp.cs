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
            string result = DeleteProjectById(dbContext);
            Console.WriteLine(result);
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();
            Project projToDelete = context
                .Projects
                .Find(2);

            EmployeeProject[] referredEmployees = context
                .EmployeesProjects
                .Where(ep => ep.ProjectId == projToDelete.ProjectId)
                .ToArray();
            context.EmployeesProjects.RemoveRange(referredEmployees);
            context.Projects.Remove(projToDelete);
            context.SaveChanges();

            string[] projectNames = context
                .Projects
                .Take(10)
                .Select(p => p.Name)
                .ToArray();
           
            foreach (var pName in projectNames)
            {
                output.AppendLine(pName);
            }
            return output.ToString().TrimEnd();
        }
    }
}
