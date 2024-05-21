﻿using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges);
        Task<Book> GetOneBookIdAsync(int id, bool trackChanges);
        void CreateOneBook(Book book);
        void UpdateOneBook(Book book);
        void DeleteOneBook(Book book);
        Task<List<Book>> GetAllBookAsync(bool trackChanges);
        Task<IEnumerable<Book>> GetAllBooksWithDetailAsync(bool trackChanges);
    }
}
