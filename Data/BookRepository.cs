using LibApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateBook(Book book)
        {
            if(book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            _context.Books.Add(book);
        }

        public List<Book> GetAllBooksWithGenre()
        {
            return _context
                .Books
                .Include(c => c.Genre)
                .ToList();
        }

        public Book GetBookById(int id)
        {
            return _context.Books.SingleOrDefault(b => b.Id == id);
        }

        public Book GetBookWithGenreById(int id)
        {
            return _context.Books.Include(b => b.Genre).SingleOrDefault(b => b.Id == id);
        }

        public void RemoveBook(Book book)
        {
            _context.Books.Remove(book);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
