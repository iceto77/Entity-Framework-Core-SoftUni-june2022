using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Problem05
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder output = new StringBuilder();
            string countrynName = Console.ReadLine();
            SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);
            sqlConnection.Open();

            string countryIdQuery = @"SELECT Id FROM Countries WHERE Name = @CountryName";
            SqlCommand countryIdCmd = new SqlCommand(countryIdQuery, sqlConnection);
            countryIdCmd.Parameters.AddWithValue("@CountryName", countrynName);
            object countryIdObj = countryIdCmd.ExecuteScalar();
            if (countryIdObj == null)
            {
                output.AppendLine($"No town names were affected.");
            }
            else
            {
                int countryId = (int)countryIdObj;
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string changeTownsNameQuery = @"UPDATE [Towns] SET [Name] = UPPER([Name]) WHERE [CountryCode] = @CountryId";
                    SqlCommand changeTownsNameCmd = new SqlCommand(changeTownsNameQuery, sqlConnection, sqlTransaction);
                    changeTownsNameCmd.Parameters.AddWithValue("@CountryId", countryId);
                    int changedTownNamesNumber = changeTownsNameCmd.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    if (changedTownNamesNumber == 0)
                    {
                        output.AppendLine($"No town names were affected.");
                    }
                    else
                    {
                        output.AppendLine($"{changedTownNamesNumber} town names were affected.");
                        string townsNameQuery = @"SELECT [Name] FROM [Towns] WHERE [CountryCode] = @CountryId";
                        SqlCommand townsNameCmd = new SqlCommand(townsNameQuery, sqlConnection);
                        townsNameCmd.Parameters.AddWithValue("@CountryId", countryId);
                        using SqlDataReader townsNameReader = townsNameCmd.ExecuteReader();
                        List<string> townsName = new List<string>();
                        while (townsNameReader.Read())
                        {
                            townsName.Add((string)townsNameReader["Name"]);
                        }
                        output.AppendLine($"[{string.Join(", ", townsName)}]"); 
                    }
                }
                catch (Exception e)
                {
                    sqlTransaction.Rollback();
                    Console.WriteLine(e.ToString());
                }
            }
            Console.WriteLine(output.ToString().TrimEnd());
            sqlConnection.Close();
        }
    }
}
