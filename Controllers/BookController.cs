using BookStore.Models;
using BookStore.Models.Repositories;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookStoreRepository<Book> bookRepository;
        private readonly IBookStoreRepository<Author> authorRepository;
        private readonly IHostingEnvironment hosting;

        public BookController(IBookStoreRepository<Book> bookRepository ,
            IBookStoreRepository<Author> authorRepository,
            IHostingEnvironment hosting)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.hosting = hosting;
        }

        // GET: BookController
        public ActionResult Index()
        {
            var books = bookRepository.List();
            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return View(model);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
           if(ModelState.IsValid)
            {
                try
                {
                                                      // coalesce operator
                    string fileName = UploadFile(model.File) ?? string.Empty;
                  
                    if (model.AuthorId == -1)
                    {
                        // dynamic property for disaly a message in it's(action) view
                        ViewBag.Message = "Please select an author from the list!";

                        return View(GetAllAuthors());
                        
                    }

                    var author = authorRepository.Find(model.AuthorId);
                    Book book = new Book
                    {
                        Id = model.BookId,
                        Title = model.Title,
                        Description = model.Discription,
                        Author = author,
                        ImageUrl = fileName
                    };

                    bookRepository.Add(book);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            ModelState.AddModelError("", " You have to fill all required fields!"); 
            return View(GetAllAuthors());
            
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookRepository.Find(id);
            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id; // ternary operator
            var viewModel = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Discription = book.Description,
                AuthorId = authorId,
                Authors = authorRepository.List().ToList(),
                ImageUrl = book.ImageUrl
            };
            return View(viewModel);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( BookAuthorViewModel viewModel)
        {
            try
            {
                // update logic coode
                string fileName = UploadFile(viewModel.File, viewModel.ImageUrl);
                    
                var author = authorRepository.Find(viewModel.AuthorId);
                var book = new Book
                {
                    Id = viewModel.BookId,
                    Title = viewModel.Title,
                    Description = viewModel.Discription,
                    Author =  author,
                    ImageUrl = fileName,
                    
                };
                bookRepository.Udpate(viewModel.BookId, book);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                bookRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // method to show a initail message in the dropdown list
        List<Author> FillSelectList()
        {
            var authors = authorRepository.List().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "-- please Select an Author --" });

                return authors;
        }

        BookAuthorViewModel GetAllAuthors()
        {
            /// for fill the list with authors
            var vmodel = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return vmodel;
        }


        // method to save the file
        string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                string FullPath = Path.Combine(uploads, file.FileName);

                file.CopyTo(new FileStream(FullPath, FileMode.Create));   // to save the file

                return file.FileName;
            }
            return null;
        }

        // overload uloadFile method
        string UploadFile(IFormFile file , string imageUrl)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");

                string newPath = Path.Combine(uploads, file.FileName);
                string oldPath = Path.Combine(uploads, imageUrl);

                // check if the image was changed so delete the old and save the new otherwise do nothing
                if (oldPath != newPath)
                {
                    System.IO.File.Delete(oldPath);

                    file.CopyTo(new FileStream(newPath, FileMode.Create));   // to save the file

                }
                return file.FileName;
            }
            return imageUrl;
        }

        public ActionResult Search(string term)
        {
            var result = bookRepository.Search(term);

            return View("Index" , result);
        }
    }
}
