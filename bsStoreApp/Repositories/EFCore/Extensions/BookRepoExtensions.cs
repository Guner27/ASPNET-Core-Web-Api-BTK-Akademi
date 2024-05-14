using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Extensions
{
    public static class BookRepoExtensions
    {
        public static IQueryable<Book> FilterBooks(this IQueryable<Book> books,
            uint minPrice, uint maxPrice) =>
            books.Where(book =>
            book.Price >= minPrice &&
            book.Price <= maxPrice);

        public static IQueryable<Book> Search(this IQueryable<Book> books, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return books;

            //Trim : Başında sonunda boşluklar varsa onları at.
            //ToLower : Tüm harfleri küçükharf yap.
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return books.Where(book => book.Title
                .ToLower()
                .Contains(searchTerm));
        }
    }
}
