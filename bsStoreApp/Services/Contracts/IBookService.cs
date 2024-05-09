using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks(bool trackChanges);
        Book GetOneBookId(int id, bool trackChanges);
        Book CreateOneBook(Book book);
        void DeleteOneBook(int id, bool trackChanges);
        void UpdateOneBook(int id, Book book, bool trackChanges);
    }
}
