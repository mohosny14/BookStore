using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class BookRepository : IBookStoreRepository<Book>
    {
        // declare list of books
        List<Book> books;
        public BookRepository()
        {
            // initialize list of books
            books = new List<Book>()
            {
                // book 1
                new Book
                {
                   Id=1,
                   Title="C# Programming" ,
                   Description="C# Descritpion" ,
                   ImageUrl="img1.jpg",
                   Author = new Author{ Id = 2}
                },
                // book 2
                new Book
                {
                   Id=2, 
                   Title="Java Programming" ,  
                   Description="Java Descritpion" ,
                   ImageUrl="img2.jpg",
                   Author = new Author()
                },
                // book 3
                new Book
                {
                   Id=3, 
                   Title="Python Programming" ,  
                   Description="Python Descritpion" ,
                   ImageUrl="img3.jpg",
                   Author = new Author()
                }
            };
        }

        // method to add books
        public void Add(Book entity)
        {
            entity.Id = books.Max(b => b.Id) + 1;
            books.Add(entity);
        }

        // method for deleting a specific book
        public void Delete(int id)
        {
            var bookFound = Find(id);
            books.Remove(bookFound);
        }

        // method to search for a specific book
        public Book Find(int id)
        {
            var bookFound = books.SingleOrDefault(b => b.Id == id);
            return bookFound;
        }

        // to return the books as a list
        public IList<Book> List()
        {
            return books;
        }

        public List<Book> Search(string term)
        {
            return books.Where(b => b.Title.Contains(term)).ToList();
        }

        // to Edit or update book
        public void Udpate(int id ,Book newBook)
        {
            var bookFound = Find(id);

            bookFound.Title = newBook.Title;
            bookFound.Description = newBook.Description;
            bookFound.Author = newBook.Author;
            bookFound.ImageUrl = newBook.ImageUrl;
        }
    }
}
