using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            var customers = GetCustomers();
            return View(customers);
        }

        public ActionResult Details(int id)
        {
            foreach (var customer in GetCustomers())
            {
                if (customer.Id == id)
                    return View(customer);
            }

            return HttpNotFound("Customer not found");
        }

        private List<Customer> GetCustomers()
        {
            var cust1 = new Customer { Id = 1, Name = "John Doe" };
            var cust2 = new Customer { Id = 2, Name = "Mary Jane" };
            return new List<Customer> {cust1, cust2};
        }
    }
}