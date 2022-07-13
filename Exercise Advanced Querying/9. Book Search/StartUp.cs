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
            Console.WriteLine(GetBookTitlesContaining(context, command));
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var bookTitle = context
                .Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();
            return String.Join(Environment.NewLine, bookTitle);
        }
    }
}
