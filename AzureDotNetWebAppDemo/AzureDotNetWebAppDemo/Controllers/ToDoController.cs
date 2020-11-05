using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureDotNetWebAppDemo.Models;

namespace AzureDotNetWebAppDemo.Controllers
{
    public class ToDoController : Controller
    {
        private ToDoDbContext dbContext = new ToDoDbContext();

        // GET: ToDo
        public ActionResult Index()
        {
            dbContext.ToDoItems.Add(new ToDoItem {Description = "Test ToDo Item 1"});
            dbContext.SaveChanges();
            return View();
        }
    }
}