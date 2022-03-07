using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class BookDbRepository : IBookStoreRepository<Book>
    {
        BookStoreDBContext db;
        public BookDbRepository(BookStoreDBContext _db)
        {
            db = _db;
        }

        // method to add books
        public void Add(Book entity)
        {
           // entity.Id = db.Books.Max(b => b.Id) + 1;
            db.Books.Add(entity);
            db.SaveChanges();
        }

        // method for deleting a specific book
        public void Delete(int id)
        {
            var bookFound = Find(id);
            db.Books.Remove(bookFound);
            db.SaveChanges();
        }

        // method to search for a specific book
        public Book Find(int id)        
        {
            var bookFound = db.Books.Include(a => a.Author).SingleOrDefault(b => b.Id == id);
            return bookFound;
        }

        // to return the books as a list
        public IList<Book> List()
        {
            return db.Books.Include(a => a.Author).ToList();
        }

        // to Edit or update book
        public void Udpate(int id, Book newBook)
        {
            db.Update(newBook);
            db.SaveChanges();
        }
        public List<Book> Search(string term)
        {
            var result = db.Books.Include(a => a.Author)
                .Where(b => b.Title.Contains(term)
                     || b.Description.Contains(term) 
                     || b.Author.FullName.Contains(term)).ToList();

            return result;
        }
    }
}
