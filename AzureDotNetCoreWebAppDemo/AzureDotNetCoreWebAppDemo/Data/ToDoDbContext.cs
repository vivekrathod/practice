using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AzureDotNetCoreWebAppDemo.Models;

namespace AzureDotNetCoreWebAppDemo.Data
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext (DbContextOptions<ToDoDbContext> options)
            : base(options)
        {
        }

        public DbSet<AzureDotNetCoreWebAppDemo.Models.ToDoItem> ToDoItem { get; set; }
    }
}
