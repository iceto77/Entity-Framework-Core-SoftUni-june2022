using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Problem04
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder output = new StringBuilder();
            string[] minionInfo = Console.ReadLine().Split(": ", StringSplitOptions.RemoveEmptyEntries)[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
            string villainName = Console.ReadLine().Split(": ", StringSplitOptions.RemoveEmptyEntries)[1];
            string minionName = minionInfo[0];
            int minionAge = int.Parse(minionInfo[1]);
            string townName = minionInfo[2];
            SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);
            sqlConnection.Open();
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                int townId = GetTownId(sqlConnection, sqlTransaction, output, townName);
                int villainId = GetTownId(sqlConnection, sqlTransaction, output, villainName);
                int minionId = AddMinion(sqlConnection, sqlTransaction, minionName, minionAge, townId);

                string addMinionToVillainQuery = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";
                SqlCommand ddMinionToVillainCmd = new SqlCommand(addMinionToVillainQuery, sqlConnection, sqlTransaction);
                ddMinionToVillainCmd.Parameters.AddWithValue("@villainId", villainId);
                ddMinionToVillainCmd.Parameters.AddWithValue("@minionId", minionId);
                output.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");
                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine(output.ToString().TrimEnd());
            sqlConnection.Close();
        }

        private static int GetTownId(SqlConnection sqlConnection, SqlTransaction sqlTransaction, StringBuilder output, string townName)
        {
            string townIdQuery = @"SELECT [Id] FROM [Towns] WHERE [Name] = @ТownName";
            SqlCommand townIdCmd = new SqlCommand(townIdQuery, sqlConnection, sqlTransaction);
            townIdCmd.Parameters.AddWithValue("@ТownName", townName);
            object townIdObj = townIdCmd.ExecuteScalar();
            if (townIdObj == null)
            {
                string addTownQuery = @"INSERT INTO [Towns]([Name]) VALUES (@TownName)";
                SqlCommand addTownCmd = new SqlCommand(addTownQuery, sqlConnection, sqlTransaction);
                addTownCmd.Parameters.AddWithValue("@TownName", townName);
                addTownCmd.ExecuteNonQuery();
                output.AppendLine($"Town {townName} was added to the database.");
                townIdObj = townIdCmd.ExecuteScalar();
            }
            int result = (int)townIdObj;
            return result;
        }

        private static int GetVillainId(SqlConnection sqlConnection, SqlTransaction sqlTransaction, StringBuilder output, string villainName)
        {
            string villainIdQuery = @"SELECT Id FROM Villains WHERE Name = @VillainName";
            SqlCommand villainIdCmd = new SqlCommand(villainIdQuery, sqlConnection, sqlTransaction);
            villainIdCmd.Parameters.AddWithValue("@VillainName", villainName);
            object villainIdObj = villainIdCmd.ExecuteScalar();
            if (villainIdObj == null)
            {
                string addVillainQuery = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
                SqlCommand addVillainCmd = new SqlCommand(addVillainQuery, sqlConnection, sqlTransaction);
                addVillainCmd.Parameters.AddWithValue("@TownName", villainName);
                addVillainCmd.ExecuteNonQuery();
                output.AppendLine($"Villain {villainName} was added to the database.");
                villainIdObj = villainIdCmd.ExecuteScalar();
            }

            return (int)villainIdObj;
        }

        private static int AddMinion(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string minionName, int minionAge, int townId)
        {
            string addMinionQuery = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@Name, @Age, @TownId)";
            SqlCommand addMinionCmd = new SqlCommand(addMinionQuery, sqlConnection, sqlTransaction);
            addMinionCmd.Parameters.AddWithValue("@Name", minionName);
            addMinionCmd.Parameters.AddWithValue("@Age", minionAge);
            addMinionCmd.Parameters.AddWithValue("@TownId", townId);
            addMinionCmd.ExecuteNonQuery();
            string addedMinionIdQuery = @"SELECT Id FROM Minions WHERE Name = @Name AND Age = @Age AND TownID = @TownId";
            SqlCommand getMinionIdCmd = new SqlCommand(addedMinionIdQuery, sqlConnection, sqlTransaction);
            getMinionIdCmd.Parameters.AddWithValue("@Name", minionName);
            getMinionIdCmd.Parameters.AddWithValue("@Age", minionAge);
            getMinionIdCmd.Parameters.AddWithValue("@TownId", townId);

            return (int)getMinionIdCmd.ExecuteScalar();
        }
    }
}
