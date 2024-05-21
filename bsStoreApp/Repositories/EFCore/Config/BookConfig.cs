using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book { Id = 1, CategoryId = 1, Title = "Another Love", Price = 72 },
                new Book { Id = 2, CategoryId = 2, Title = "Prince Of Persia", Price = 70 },
                new Book { Id = 3, CategoryId = 1, Title = "My Love", Price = 81 }
                );
        }
    }
}
