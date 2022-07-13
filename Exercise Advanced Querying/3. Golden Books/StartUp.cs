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
            Console.WriteLine(GetGoldenBooks(context));
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            string[] bookTitles = context
                .Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();
            return String.Join(Environment.NewLine, bookTitles);
        }
    }
}
