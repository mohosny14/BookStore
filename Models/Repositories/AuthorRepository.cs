using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class AuthorRepository : IBookStoreRepository<Author>
    {
        IList<Author> authors;
        public AuthorRepository()
        {
            authors = new List<Author>()
            {
                new Author {Id = 1 , FullName = "Mohamed Hosny"},
                new Author {Id = 2 , FullName = "Zein Hosam"},
                new Author {Id = 3 , FullName = "Hatem Hosny"},
            };
        }
        public void Add(Author entity)
        {
           // entity.Id = authors.Max(a => a.Id) + 1;
            authors.Add(entity);
        }

        public void Delete(int id)
        {
            var author = Find(id);
            authors.Remove(author);
        }

        public Author Find(int id)
        {
            var authorSearched = authors.SingleOrDefault(a => a.Id == id);
            return authorSearched;
        }

        public IList<Author> List()
        {
            return authors;
        }

        public List<Author> Search(string term)
        {
            return authors.Where(b => b.FullName.Contains(term)).ToList();
        }

        public void Udpate(int id, Author newAuthor)
        {
            var author = Find(id);

            author.FullName = newAuthor.FullName;

        }
    }
}
