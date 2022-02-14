using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibApp.Models;
using LibApp.ViewModels;
using LibApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;

        public BooksController(IBookRepository bookRepository, IGenreRepository genreRepository)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
        }

        public ViewResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var book = _bookRepository.GetBookWithGenreById(id);

            return View(book);
        }

        public IActionResult Edit(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BookFormViewModel
            {
                Book = book,
                Genres = _genreRepository.GetAllGenres(),
            };

            return View("BookForm", viewModel);
        }

        public IActionResult New()
        {
            var genres = _genreRepository.GetAllGenres();

            var viewModel = new BookFormViewModel
            {
                Book = new Book(),
                Genres = genres,
            };

            return View("BookForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Book book)
        {
            if(!ModelState.IsValid)
            {
                var viewModel = new BookFormViewModel()
                {
                    Book = book,
                    Genres = _genreRepository.GetAllGenres(),
                };

                return View("BookForm", viewModel);
            }

            if (book.Id == 0)
            {
                book.DateAdded = DateTime.Now;
                _bookRepository.CreateBook(book);
            }
            else
            {
                var bookInDb = _bookRepository.GetBookById(book.Id);
                bookInDb.Name = book.Name;
                bookInDb.AuthorName = book.AuthorName;
                bookInDb.GenreId = book.GenreId;
                bookInDb.ReleaseDate = book.ReleaseDate;
                bookInDb.DateAdded = book.DateAdded;
                bookInDb.NumberInStock = book.NumberInStock;
            }

            try
            {
                _bookRepository.SaveChanges();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            

            return RedirectToAction("Index", "Books");
        }
    }
}
