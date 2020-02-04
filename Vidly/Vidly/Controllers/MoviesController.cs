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
        // GET: Movies/Random
        public ActionResult Random()
        {
            var movie = new Movie { Id = 1, Name = "Shrek!" };
            return View(movie);
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

        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content("year=" + year + "&month=" + month);
        }
    }
}