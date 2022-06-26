using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Problem07
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder output = new StringBuilder();
            using SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);
            sqlConnection.Open();
            string minionsNameQuery = @"SELECT [Name] FROM [Minions]";
            SqlCommand minionsNameCmd = new SqlCommand(minionsNameQuery, sqlConnection);
            using SqlDataReader minionsNameReader = minionsNameCmd.ExecuteReader();
            Queue<string> minionsNameQueue = new Queue<string>();
            Stack<string> minionsNameStack = new Stack<string>();
            List<string> minionsName = new List<string>();
            while (minionsNameReader.Read())
            {
                minionsNameQueue.Enqueue((string)minionsNameReader["Name"]);
                minionsNameStack.Push((string)minionsNameReader["Name"]);
            }
            int minionsCount = minionsNameQueue.Count;
            for (int i = 0; i < minionsCount; i++)
            {
                if (i % 2 == 0)
                {
                    minionsName.Add(minionsNameQueue.Dequeue());
                }
                else
                {
                    minionsName.Add(minionsNameStack.Pop());
                }
            }
            output.AppendLine(string.Join("\n", minionsName));
            Console.WriteLine(output.ToString().TrimEnd());
            sqlConnection.Close();
        }
    }
}
