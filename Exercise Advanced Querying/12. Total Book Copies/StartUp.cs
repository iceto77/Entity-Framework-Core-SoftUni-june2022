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
            Console.WriteLine(CountCopiesByAuthor(context));
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder output = new StringBuilder();
            var bookInfo = context
                .Authors
                .Select(a => new
                {
                    AuthorName = $"{a.FirstName} {a.LastName}",
                    booksCount = a
                    .Books
                    .Sum(b => b.Copies)
                })
                .ToArray()
                .OrderByDescending(x => x.booksCount)
                .ToArray();                

            foreach (var b in bookInfo)
            {
                output.AppendLine($"{b.AuthorName} - {b.booksCount}");
            }

            return output.ToString().TrimEnd();
        }
    }
}
