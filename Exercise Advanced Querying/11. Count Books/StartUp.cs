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
            Console.WriteLine(CountBooks(context, command));
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int numbersOfBooks = context
                .Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToArray()
                .Count();
                
            return numbersOfBooks;
        }
    }
}
