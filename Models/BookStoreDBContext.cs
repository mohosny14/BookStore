using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class BookStoreDBContext : DbContext
    {
        public BookStoreDBContext(DbContextOptions<BookStoreDBContext> options) : base(options)
        {

        }

        // Classes gonna to be tables in the database
        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }
    }
}
