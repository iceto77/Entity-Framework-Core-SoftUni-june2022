using System;
using System.Data.SqlClient;
using System.Text;

namespace Problem03
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder output = new StringBuilder();
            int villainId = int.Parse(Console.ReadLine());
            using SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);
            sqlConnection.Open();
            string villainNameQuery = @"SELECT Name FROM Villains WHERE Id = @VillainId";
            SqlCommand villainNameCmd = new SqlCommand(villainNameQuery, sqlConnection);
            villainNameCmd.Parameters.AddWithValue("@VillainId", villainId);
            string villainName = (string)villainNameCmd.ExecuteScalar();
            if (villainName == null)
            {
                output.AppendLine($"No villain with ID {villainId} exists in the database.");
            }
            else
            {
                output.AppendLine($"Villain: {villainName}");
                string minionsQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                               m.Name, 
                                               m.Age
                                          FROM MinionsVillains AS mv
                                          JOIN Minions As m ON mv.MinionId = m.Id
                                         WHERE mv.VillainId = @VillainId
                                      ORDER BY m.Name";
                SqlCommand minionsCmd = new SqlCommand(minionsQuery, sqlConnection);
                minionsCmd.Parameters.AddWithValue("@VillainId", villainId);
                using SqlDataReader minionsReader = minionsCmd.ExecuteReader();
                if (!minionsReader.HasRows)
                {
                    output.AppendLine($"(no minions)");
                }
                else
                {
                    int row = 1;
                    while (minionsReader.Read())
                    {
                        output.AppendLine($"{row++}. {minionsReader["Name"]} {minionsReader["Age"]}");
                    }
                }
            }
            Console.WriteLine(output.ToString().TrimEnd());
            sqlConnection.Close();
        }
    }
}
