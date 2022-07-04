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
            string result = GetLatestProjects(dbContext);
            Console.WriteLine(result);
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder output = new StringBuilder();
            var last10Projects = context
                .Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    ProjectName = p.Name,
                    ProjectDescription = p.Description,
                    ProjectStartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt")
                })
                .OrderBy(p => p.ProjectName)
                .ToArray();
            foreach (var p in last10Projects)
            {
                output.AppendLine($"{p.ProjectName}{Environment.NewLine}{p.ProjectDescription}{Environment.NewLine}{p.ProjectStartDate}");                
            }
            return output.ToString().TrimEnd();
        }
    }
}
