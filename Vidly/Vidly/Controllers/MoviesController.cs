using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies/Index
        public ActionResult Index()
        {
            var movie1 = new Movie { Name = "Shrek!" };
            var movie2 = new Movie { Name = "Wall-e" };
            var moviesList = new List<Movie> {movie1, movie2};
            return View(moviesList);
        }
    }
}