using LibApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Data
{
    public class RentalRepository : IRentalRepository
    {
        private readonly ApplicationDbContext _context;

        public RentalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateRental(Rental rental)
        {
            _context.Rentals.Add(rental);
        }

        public IEnumerable<Rental> FindRentalsByBookId(int id)
        {
            return _context.Rentals.Where(r => r.Book.Id == id);
        }

        public void RemoveRental(Rental rental)
        {
            _context.Rentals.Remove(rental);
        }
    }
}
