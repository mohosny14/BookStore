using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class AuthorDbRepository : IBookStoreRepository<Author>
    {
        BookStoreDBContext db;
        public AuthorDbRepository( BookStoreDBContext _db)
        {
            db = _db;
        }
        public void Add(Author entity)
        {
            
            db.Authors.Add(entity);
            // need to refactor because it's repeated in more than one place ( use method and call it instade that )
            db.SaveChanges();

        }

        public void Delete(int id)
        {
            var author = Find(id);
            db.Authors.Remove(author);
            db.SaveChanges(); 

        }

        public Author Find(int id)
        {
            var authorSearched = db.Authors.SingleOrDefault(a => a.Id == id);
            return authorSearched;
        }

        public IList<Author> List()
        {
            return db.Authors.ToList();
        }

        public List<Author> Search(string term)
        {
            return db.Authors.Where(b => b.FullName.Contains(term)).ToList();
        }

        public void Udpate(int id, Author newAuthor)
        {
            db.Update(newAuthor);
            db.SaveChanges();
        }
    }
}
