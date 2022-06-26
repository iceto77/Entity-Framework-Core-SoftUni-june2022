using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Problem08
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder output = new StringBuilder();
            int[] minionsId = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);
            sqlConnection.Open();
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                for (int i = 0; i < minionsId.Length; i++)
                {
                    string updateMinionInfoQuery = @"UPDATE [Minions] 
                                                        SET [Name] = UPPER(LEFT([Name], 1)) + SUBSTRING([Name], 2, LEN([Name])), 
                                                            [Age] += 1 
                                                      WHERE [Id] = @MinionId";
                    SqlCommand updateMinionInfoCmd = new SqlCommand(updateMinionInfoQuery, sqlConnection, sqlTransaction);
                    updateMinionInfoCmd.Parameters.AddWithValue("@MinionId", minionsId[i]);
                    updateMinionInfoCmd.ExecuteNonQuery();
                }
                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                Console.WriteLine(e.ToString());
            }
            string minionsInfoQuery = @"SELECT [Name], [Age] FROM [Minions]";
            SqlCommand minionsInfoCmd = new SqlCommand(minionsInfoQuery, sqlConnection);
            using SqlDataReader minionsInfoReader = minionsInfoCmd.ExecuteReader();
            while (minionsInfoReader.Read())
            {
                output.AppendLine($"{(string)minionsInfoReader["Name"]} {(int)minionsInfoReader["Age"]}");
            }
            Console.WriteLine(output.ToString().TrimEnd());
            sqlConnection.Close();
        }
    }
}