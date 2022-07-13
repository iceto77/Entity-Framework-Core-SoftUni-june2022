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
            //DbInitializer.ResetDatabase(db);
            //string command = Console.ReadLine();
            //int command = int.Parse(Console.ReadLine());
            //Console.WriteLine(CountBooks(context, command));
            Console.WriteLine(RemoveBooks(context));
            //IncreasePrices(context);
        }

        //Problem 02
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

        //Problem 03
        public static string GetGoldenBooks(BookShopContext context)
        {
            string[] bookTitles = context
                .Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();
            return String.Join(Environment.NewLine, bookTitles);
        }

        //Problem 04
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

        //Problem 05
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

        //Problem 06
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower()).ToArray();
            var booksFromCategories = context
                .Books
                .Where(b => b.BookCategories.Any(b => categories.Contains(b.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();
            return String.Join(Environment.NewLine, booksFromCategories);
        }

        //Problem 07
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

        //Problem 08
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

        //Problem 09
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

        //Problem 10
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder output = new StringBuilder();
            var bookInfo = context
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    AutorName = $"{b.Author.FirstName} {b.Author.LastName}"
                })
                .ToArray();
            foreach (var b in bookInfo)
            {
                output.AppendLine($"{b.Title} ({b.AutorName})");
            }

            return output.ToString().TrimEnd();
        }

        //Problem 11
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int numbersOfBooks = context
                .Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToArray()
                .Count();
                
            return numbersOfBooks;
        }

        //Problem 12
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder output = new StringBuilder();
            var bookInfo = context
                .Authors
                .Select(a => new
                {
                    AuthorName = $"{a.FirstName} {a.LastName}",
                    booksCount = a
                    .Books
                    .Sum(b => b.Copies)
                })
                .ToArray()
                .OrderByDescending(x => x.booksCount)
                .ToArray();                

            foreach (var b in bookInfo)
            {
                output.AppendLine($"{b.AuthorName} - {b.booksCount}");
            }

            return output.ToString().TrimEnd();
        }

        //Problem 13
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

        //Problem 14
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder output = new StringBuilder();
            var mostRecentBookByCategory = context
                .Categories
                .Select(c => new
                {
                    Category = c.Name,
                    Books = c.CategoryBooks
                    .OrderByDescending(cb => cb.Book.ReleaseDate.Value)
                    .Select(cb => new
                    {
                        BookTitle = cb.Book.Title,
                        BookReleaseYear = cb.Book.ReleaseDate.Value.Year
                    })
                    .Take(3)
                    .ToArray()
                })
                .OrderBy(c => c.Category)
                .ToArray();

            foreach (var catg in mostRecentBookByCategory)
            {
                output.AppendLine($"--{catg.Category}");
                foreach (var b in catg.Books)
                {
                    output.AppendLine($"{b.BookTitle} ({b.BookReleaseYear})");
                }
            }

            return output.ToString().TrimEnd();
        }

        //Problem 15
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

        //Problem 16
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
