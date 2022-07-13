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
            Console.WriteLine(GetMostRecentBooks(context));
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder output = new StringBuilder();
            var mostRecentBookByCategory = context
                .Categories
                .Select(c => new
                {
                    Category = c.Name,
                    Books = c.CategoryBooks
                    .OrderByDescending(cb => cb.Book.ReleaseDate.Value)
                    .Select(cb => new
                    {
                        BookTitle = cb.Book.Title,
                        BookReleaseYear = cb.Book.ReleaseDate.Value.Year
                    })
                    .Take(3)
                    .ToArray()
                })
                .OrderBy(c => c.Category)
                .ToArray();

            foreach (var catg in mostRecentBookByCategory)
            {
                output.AppendLine($"--{catg.Category}");
                foreach (var b in catg.Books)
                {
                    output.AppendLine($"{b.BookTitle} ({b.BookReleaseYear})");
                }
            }

            return output.ToString().TrimEnd();
        }
    }
}
