using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Customers
        public ActionResult Index()
        {
            var customers = _context.Customers.Include(c => c.MembershipType);
            return View(customers);
        }

        public ActionResult Details(int id)
        {
            foreach (var customer in _context.Customers.Include(c => c.MembershipType))
            {
                if (customer.Id == id)
                    return View(customer);
            }

            return HttpNotFound("Customer not found");
        }

        public ViewResult New()
        {
            var memTypes = _context.MembershipTypes.ToList();
            var newCustomerViewModel = new CustomerFormViewModel {MembershipTypes = memTypes};
            return View("CustomerForm", newCustomerViewModel);
        }

        [HttpPost]
        public ActionResult Save(Customer customer)
        {
            if (customer.Id == 0)
                _context.Customers.Add(customer);
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);
                customerInDb.Name = customer.Name;
                customerInDb.BirthDate = customer.BirthDate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Customers");
        }

        public ActionResult Edit(int id)
        {
            var cust = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (cust == null)
                return new HttpNotFoundResult();
            var viewModel = new CustomerFormViewModel
                {Customer = cust, MembershipTypes = _context.MembershipTypes.ToList()};
            return View("CustomerForm", viewModel);
        }
    }
}