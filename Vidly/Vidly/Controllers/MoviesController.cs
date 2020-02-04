using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies/Random
        public ActionResult Random()
        {
            var movie = new Movie { Name = "Shrek!" };
            var customers = new List<Customer>
            {
                new Customer {Name = "Customer1"}, 
                new Customer {Name = "Customer2"}
            };

            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };

            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            return Content("Id= " + id);
        }

        public ActionResult Index(int? pageIndex, string sortBy)
        {
            if (!pageIndex.HasValue)
                pageIndex = 1;

            if (sortBy.IsNullOrWhiteSpace())
                sortBy = "Name";

            return Content("pageIndex=" + pageIndex + "&sortBy=" + sortBy);
        }

        // \note the '?' at the end of 'year' makes it optional 
        // this allows for the route to match both http://localhost:62444/movies/released/2018/09 as well as http://localhost:62444/movies/released?year=2018&month=09
        [Route("movies/released/{year?}/{month?}")]
        public ActionResult ByReleaseDate(int? year, int? month)
        {
            return Content("year=" + year + "&month=" + month);
        }
    }
}