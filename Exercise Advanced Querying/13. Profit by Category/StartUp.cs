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
            Console.WriteLine(GetTotalProfitByCategory(context));
        }
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder output = new StringBuilder();
            var profitByCategoty = context
                .Categories
                .Select(c => new
                {
                    c.Name,
                    Profit = c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .ToArray()
                .OrderByDescending(x => x.Profit)
                .ToArray();
            foreach (var p in profitByCategoty)
            {
                output.AppendLine($"{p.Name} ${p.Profit:f2}");
            }

            return output.ToString().TrimEnd();
        }
    }
}
