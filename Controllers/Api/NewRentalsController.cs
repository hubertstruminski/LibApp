using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewRentalsController : ControllerBase
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBookRepository _bookRepository;

        public NewRentalsController(IRentalRepository rentalRepository, ICustomerRepository customerRepository, 
            IBookRepository bookRepository)
        {
            _rentalRepository = rentalRepository;
            _customerRepository = customerRepository;
            _bookRepository = bookRepository;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateNewRental([FromBody] NewRentalDto newRental)
        {
            var customer = _customerRepository
                .GetAllCustomersWithMembershipType()
                .Single(c => c.Id == newRental.CustomerId);

            var books = _bookRepository
                .GetAllBooksWithGenre()
                .Where(b => newRental.BookIds.Contains(b.Id));

            foreach (var book in books)
            {
                if (book.NumberAvailable == 0)
                    return BadRequest("Book is not available");

                book.NumberAvailable--;
                var rental = new Rental()
                {
                    Customer = customer,
                    Book = book,
                    DateRented = DateTime.Now,
                };
                _rentalRepository.CreateRental(rental);
            }
            _customerRepository.SaveChanges();

            return Ok();
        }
    }
}
