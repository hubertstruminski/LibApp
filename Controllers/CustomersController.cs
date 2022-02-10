using LibApp.Data;
using LibApp.Models;
using LibApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMembershipTypeRepository _membershipTypeRepository;

        public CustomersController(ICustomerRepository customerRepository, IMembershipTypeRepository membershipTypeRepository)
        {
            _customerRepository = customerRepository;
            _membershipTypeRepository = membershipTypeRepository;
        }

        public ViewResult Index()
        { 
            return View();
        }

        public IActionResult Details(int id)
        {
            var customer = _customerRepository.GetCustomerWithMembershipTypeById(id);

            if (customer == null)
            {
                return Content("User not found");
            }

            return View(customer);
        }

        public IActionResult New()
        {
            var membershipTypes = _membershipTypeRepository.GetAllMembershipTypes();
            var viewModel = new CustomerFormViewModel()
            {
                MembershipTypes = membershipTypes,
            };

            return View("CustomerForm", viewModel);
        }

        public IActionResult Edit(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);

            if(customer == null)
            {
                return NotFound();
            }

            var viewModel = new CustomerFormViewModel(customer)
            {
                MembershipTypes = _membershipTypeRepository.GetAllMembershipTypes(),
            };

            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Customer customer)
        {
            if(!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel(customer)
                {
                    MembershipTypes = _membershipTypeRepository.GetAllMembershipTypes(),
                };

                return View("CustomerForm", viewModel);
            }

            if(customer.Id == 0)
            {
                _customerRepository.CreateCustomer(customer);
            }
            else
            {
                var customerInDb = _customerRepository.GetCustomerById(customer.Id);
                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.HasNewsletterSubscribed = customer.HasNewsletterSubscribed;
            }
            _customerRepository.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }
    }
}
