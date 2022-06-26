using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Problem09
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder output = new StringBuilder();
            int minionId = int.Parse(Console.ReadLine());
            SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);
            sqlConnection.Open();

            string increaseAgeQuery = @"EXEC [dbo].[usp_GetOlder] @MinionId";
            SqlCommand increaseAgeCmd = new SqlCommand(increaseAgeQuery, sqlConnection);
            increaseAgeCmd.Parameters.AddWithValue("@MinionId", minionId);
            increaseAgeCmd.ExecuteNonQuery();
            string minionInfoQuery = @"SELECT [Name], [Age] FROM [Minions] WHERE [Id] = @MinionId";
            SqlCommand minionInfoCmd = new SqlCommand(minionInfoQuery, sqlConnection);
            minionInfoCmd.Parameters.AddWithValue("@MinionId", minionId);
            using SqlDataReader infoReader = minionInfoCmd.ExecuteReader();
            while (infoReader.Read())
            {
                output.AppendLine($"{infoReader["Name"]} – {infoReader["Age"]} years old");
            }
            Console.WriteLine(output.ToString().TrimEnd());
            sqlConnection.Close();
        }
    }
}
