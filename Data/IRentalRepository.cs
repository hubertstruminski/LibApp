using LibApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Data
{
    public interface IRentalRepository
    {
        IEnumerable<Rental> FindRentalsByBookId(int id);

        void RemoveRental(Rental rental);

        void CreateRental(Rental rental);
    }
}
