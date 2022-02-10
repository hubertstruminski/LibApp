using LibApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateCustomer(Customer customer)
        {
            if(customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            _context.Customers.Add(customer);
        }

        public Customer FindCustomerById(int id)
        {
            return _context.Customers.Where(c => c.Id == id).FirstOrDefault();
        }

        public List<Customer> GetAllCustomersWithMembershipType()
        {
            return _context
                .Customers
                .Include(c => c.MembershipType)
                .ToList();
        }

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.SingleOrDefault(c => c.Id == id);
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);
        }

        public Customer GetCustomerWithMembershipTypeById(int id)
        {
            return _context.Customers
               .Include(c => c.MembershipType)
               .SingleOrDefault(c => c.Id == id);
        }

        public void RemoveCustomer(Customer customer)
        {
            _context.Customers.Remove(customer);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
