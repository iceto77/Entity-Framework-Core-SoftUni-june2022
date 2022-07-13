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
            Console.WriteLine(GetBooksByCategory(context, command));
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower()).ToArray();
            var booksFromCategories = context
                .Books
                .Where(b => b.BookCategories.Any(b => categories.Contains(b.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();
            return String.Join(Environment.NewLine, booksFromCategories);
        }
    }
}
