using LibApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Data
{
    public interface ICustomerRepository
    {
        Customer GetCustomerById(int id);

        Task<Customer> GetCustomerByIdAsync(int id);

        Customer GetCustomerWithMembershipTypeById(int id);

        void CreateCustomer(Customer customer);

        bool SaveChanges();

        Customer FindCustomerById(int id);

        List<Customer> GetAllCustomersWithMembershipType();

        void RemoveCustomer(Customer customer);
    }
}
