namespace BookShop
{
    using Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using BookShop.Models;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
	    string command = Console.ReadLine();
            Console.WriteLine(GetBooksByAuthor(context, command));
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder output = new StringBuilder();
            var bookInfo = context
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    AutorName = $"{b.Author.FirstName} {b.Author.LastName}"
                })
                .ToArray();
            foreach (var b in bookInfo)
            {
                output.AppendLine($"{b.Title} ({b.AutorName})");
            }

            return output.ToString().TrimEnd();
        }
    }
}
