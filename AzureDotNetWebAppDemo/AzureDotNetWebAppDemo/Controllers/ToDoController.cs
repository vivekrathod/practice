using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AzureDotNetWebAppDemo.Models;
using AzureDotNetWebAppDemo.ViewModels;

namespace AzureDotNetWebAppDemo.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoDbContext _dbContext = new ToDoDbContext();

        // GET: ToDo
        public ActionResult Index()
        {
            var items = _dbContext.ToDoItems.Select(item => 
                    new ToDoItemViewModel
                    {
                        Id = item.Id,
                        Description = item.Description, 
                        StartDate = item.StartDate,
                        Finished = item.Finished
                    });
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ToDoItemViewModel itemViewModel)
        {
            var items = _dbContext.ToDoItems;
            if (ModelState.IsValid)
            {
                var item = new ToDoItem
                    {
                        Description = itemViewModel.Description, 
                        StartDate = itemViewModel.StartDate,
                        Finished = itemViewModel.Finished
                    }
                    ;
                items.Add(item);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var item = _dbContext.ToDoItems.Find(id);
            if (item == null)
                return HttpNotFound();

            ToDoItemViewModel itemViewModel = new ToDoItemViewModel
            {
                Id = id,
                Description = item.Description,
                StartDate = item.StartDate,
                Finished = item.Finished
            };

            return View("Edit", itemViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ToDoItemViewModel itemViewModel)
        {
            if (ModelState.IsValid)
            {
                var item = _dbContext.ToDoItems.Find(itemViewModel.Id);
                if (item != null)
                {
                    item.Description = itemViewModel.Description;
                    item.StartDate = itemViewModel.StartDate;
                    item.Finished = itemViewModel.Finished;
                }

                _dbContext.SaveChanges();
            }

            return View(itemViewModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var item = _dbContext.ToDoItems.Find(id);
            if (item == null)
                return HttpNotFound();

            var itemViewModel = new ToDoItemViewModel
            {
                Id = item.Id, 
                Description = item.Description, 
                StartDate = item.StartDate,
                Finished = item.Finished
            };
            return View(itemViewModel);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var item = _dbContext.ToDoItems.Find(id);
            if (item == null)
                return HttpNotFound();

            _dbContext.ToDoItems.Remove(item);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}