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
            Console.WriteLine(GetAuthorNamesEndingIn(context, command));
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorNames = context
                .Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToArray()
                .OrderBy(n => n)
                .ToArray();
            return String.Join(Environment.NewLine, authorNames);
        }
    }
}
