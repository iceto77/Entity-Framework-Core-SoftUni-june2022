using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Problem06
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder output = new StringBuilder();
            int villainId = int.Parse(Console.ReadLine());
            SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);
            sqlConnection.Open();
            string villainNameQuery = @"SELECT [Name] FROM [Villains] WHERE [Id] = @VillainId";
            SqlCommand villainNameCmd = new SqlCommand(villainNameQuery, sqlConnection);
            villainNameCmd.Parameters.AddWithValue("@VillainId", villainId);
            string villainName = (string)villainNameCmd.ExecuteScalar();
            if (villainName == null)
            {
                output.AppendLine($"No such villain was found.");
            }
            else
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                string releaseMinionsQuery = @"DELETE FROM [MinionsVillains] WHERE [VillainId] = @VillainId";
                SqlCommand releaseMinionsCmd = new SqlCommand(releaseMinionsQuery, sqlConnection, sqlTransaction);
                releaseMinionsCmd.Parameters.AddWithValue("@VillainId", villainId);
                int minionsReleased = releaseMinionsCmd.ExecuteNonQuery();
                string deleteVillainQuery = @"DELETE FROM [Villains] WHERE [Id] = @VillainId";
                SqlCommand deleteVillainCmd = new SqlCommand(deleteVillainQuery, sqlConnection, sqlTransaction);
                deleteVillainCmd.Parameters.AddWithValue("@VillainId", villainId);
                int villainsDeleted = deleteVillainCmd.ExecuteNonQuery();
                if (villainsDeleted != 1)
                {
                    sqlTransaction.Rollback();
                }
                else
                {
                    sqlTransaction.Commit();
                    output.AppendLine($"{villainName} was deleted.");
                    output.AppendLine($"{minionsReleased} minions were released.");
                }
            }
            Console.WriteLine(output.ToString().TrimEnd());
            sqlConnection.Close();
        }
    }
}
