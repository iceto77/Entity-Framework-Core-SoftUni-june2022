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
            Console.WriteLine(RemoveBooks(context));        
        }
        
        public static int RemoveBooks(BookShopContext context)
        {
            IQueryable<Book> booksForRemove = context
                .Books
                .Where(b => b.Copies < 4200);
            int count = 0;
            foreach (var book in booksForRemove)
            {
                context.Books.Remove(book);
                count++;
            }
            context.SaveChanges();
            return count;
        }
    }
}
