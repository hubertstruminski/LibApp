using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace LibApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}/details")]
        public IActionResult GetCustomerDetails(int id)
        {
            return Ok(id);
        }

        // GET /api/customers
        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _customerRepository
                .GetAllCustomersWithMembershipType()
                .Select(_mapper.Map<Customer, CustomerDto>);

            return Ok(customers);
        }

        // GET /api/customers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CustomerDto>(customer));
        }

        // POST /api/customers
        [HttpPost]
        public CustomerDto CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var customer = _mapper.Map<Customer>(customerDto);

            _customerRepository.CreateCustomer(customer);
            _customerRepository.SaveChanges();

            customerDto.Id = customer.Id;

            return customerDto;
        }

        // PUT /api/customers/{id}
        [HttpPut("{id}")]
        public void UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var customerInDb = _customerRepository.GetCustomerById(id);
            if (customerInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _mapper.Map(customerDto, customerInDb);
            // TUTAJ POWINIEN BYC _customerRepository.Update(obj); SPRAWDZIĆ!!!
            _customerRepository.SaveChanges();
        }

        // DELETE /api/customers
        [HttpDelete("{id}")]
        public void DeleteCustomer(int id)
        {
            var customerInDb = _customerRepository.GetCustomerById(id);
            if (customerInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _customerRepository.RemoveCustomer(customerInDb);
            _customerRepository.SaveChanges();
        } 
    }
}