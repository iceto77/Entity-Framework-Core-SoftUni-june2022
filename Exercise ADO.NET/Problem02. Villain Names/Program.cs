using System;
using System.Data.SqlClient;
using System.Text;

namespace Problem02
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder output = new StringBuilder();
            using SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);
            sqlConnection.Open();
            string query = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount 
                              FROM Villains AS v
                              JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                          GROUP BY v.Id, v.Name
                            HAVING COUNT(mv.VillainId) > 3
                          ORDER BY COUNT(mv.VillainId)";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                output.AppendLine($"{reader["Name"]} - {reader["MinionsCount"]}");
            }

            //output.ToString().TrimEnd();
            Console.WriteLine(output.ToString().TrimEnd());
            sqlConnection.Close();
        }
    }
}
