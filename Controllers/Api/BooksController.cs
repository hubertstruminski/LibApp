using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Microsoft.AspNetCore.Authorization;

namespace LibApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository, IRentalRepository rentalRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}/details")]
        public IActionResult GetBookDetails(int id)
        {
            return Ok(id);
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _bookRepository.GetAllBooksWithGenre().Select(_mapper.Map<Book, BookDto>);

            return Ok(books);
        }

        [HttpDelete("{id}")]
        public void DeleteBook(int id)
        {
            var bookInDb = _bookRepository.GetBookById(id);
            if(bookInDb == null)
            {
                throw new System.Web.Http.HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            IEnumerable<Rental> rentals = _rentalRepository.FindRentalsByBookId(id);
            foreach (var rental in rentals)
            {
                _rentalRepository.RemoveRental(rental);
            }

            _bookRepository.RemoveBook(bookInDb);
            _bookRepository.SaveChanges();
        }

        // GET api/books
        //[HttpGet]
        //public IEnumerable<BookDto> GetBooks(string query = null)
        //{
        //    var booksQuery = _context.Books.Where(b => b.NumberAvailable > 0);

        //    if(!String.IsNullOrWhiteSpace(query))
        //    {
        //        booksQuery = booksQuery.Where(b => b.Name.Contains(query));
        //    }

        //    return booksQuery.ToList().Select(_mapper.Map<Book, BookDto>);
        //}
    }
}
