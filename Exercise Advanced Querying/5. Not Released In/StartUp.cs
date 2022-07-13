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
            int command = int.Parse(Console.ReadLine());
            Console.WriteLine(GetBooksNotReleasedIn(context, command));
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var notReleasedBook = context
                .Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();
            return String.Join(Environment.NewLine, notReleasedBook);
        }
    }
}
