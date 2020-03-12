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
            var newCustomerViewModel = new NewCustomerViewModel {MembershipTypes = memTypes};
            return View(newCustomerViewModel);
        }

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return RedirectToAction("Index", "Customers");
        }
    }
}