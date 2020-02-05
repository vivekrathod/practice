using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TODOWebApp.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Description { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DueBy { get; set; }

        public string Tag { get; set; }

        public bool InProgress { get; set; }
    }

    public class ToDoItemDbContext : DbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}