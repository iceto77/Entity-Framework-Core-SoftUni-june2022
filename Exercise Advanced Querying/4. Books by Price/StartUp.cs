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
            Console.WriteLine(GetBooksByPrice(context));
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder output = new StringBuilder();
            var bookInfo = context
                .Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .ToArray();
            foreach (var b in bookInfo)
            {
                output.AppendLine($"{b.Title} - ${b.Price:f2}");
            }   

            return output.ToString().TrimEnd();
        }
    }
}
