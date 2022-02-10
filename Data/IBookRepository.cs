using LibApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Data
{
    public interface IBookRepository
    {
        Book GetBookById(int id);

        void CreateBook(Book book);

        bool SaveChanges();

        Book GetBookWithGenreById(int id);

        List<Book> GetAllBooksWithGenre();

        void RemoveBook(Book book);
    }
}
