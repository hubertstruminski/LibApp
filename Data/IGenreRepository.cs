using LibApp.Models;
using System.Collections.Generic;

namespace LibApp.Data
{
    public interface IGenreRepository
    {
        List<Genre> GetAllGenres();
    }
}
