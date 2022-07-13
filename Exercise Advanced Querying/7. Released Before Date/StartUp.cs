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
            Console.WriteLine(GetBooksReleasedBefore(context, command));
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder output = new StringBuilder();
            var targetDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var bookInfo = context
                .Books
                .Where(b => b.ReleaseDate < targetDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToArray();

            foreach (var b in bookInfo)
            {
                output.AppendLine($"{b.Title} - {b.EditionType} - ${b.Price:f2}");
            }

            return output.ToString().TrimEnd();
        }
    }
}
