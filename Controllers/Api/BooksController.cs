using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace LibApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _context
                .Books
                .Include(c => c.Genre)
                .ToList()
                .Select(_mapper.Map<Book, BookDto>);
            return Ok(books);
        }

        [HttpDelete("{id}")]
        public void DeleteBook(int id)
        {
            var bookInDb = _context.Books.SingleOrDefault(c => c.Id == id);
            if(bookInDb == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            IEnumerable<Rental> rentals = _context.Rentals.Where(r => r.Book.Id == id);
            foreach(var rental in rentals)
            {
                _context.Rentals.Remove(rental);
            }

            _context.Books.Remove(bookInDb);
            _context.SaveChanges();
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
