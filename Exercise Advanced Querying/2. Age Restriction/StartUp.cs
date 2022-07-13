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
            DbInitializer.ResetDatabase(db);
            string command = Console.ReadLine();
            Console.WriteLine(GetBooksByAgeRestriction(context, command));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            AgeRestriction ageRestriction;
            bool parseSuccess = Enum.TryParse<AgeRestriction>(command, true, out ageRestriction);
            if (!parseSuccess)
            {
                return String.Empty;
            }
            string[] bookTitles = context
                .Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();
            return String.Join(Environment.NewLine, bookTitles);
        }
    }
}
