using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AzureDotNetWebAppDemo.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        public bool Finished { get; set; }
    }

    public class ToDoDbContext : DbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }

        public ToDoDbContext() : base("ToDoContextConnectionString")
        {
            
        }
    }
}