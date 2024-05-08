using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Repositories.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book { Id=1,Title="Another Love", Price=72},
                new Book { Id=2,Title="Prince Of Persia", Price=70},
                new Book { Id=3,Title="My Love", Price=81}
                );
        }
    }
}
