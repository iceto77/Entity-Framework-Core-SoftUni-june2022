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
            IncreasePrices(context);
        }
        
        public static void IncreasePrices(BookShopContext context)
        {
            IQueryable<Book> booksBeforeYear = context
                .Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);
            foreach (var book in booksBeforeYear)
            {
                book.Price += 5;
            }
            context.SaveChanges();
        }
    }
}
