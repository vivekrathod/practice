using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AzureDotNetCoreWebAppDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AzureDotNetCoreWebAppDemo.Data
{
    public class ToDoDbContext : IdentityDbContext<IdentityUser>
    {
        public ToDoDbContext (DbContextOptions<ToDoDbContext> options)
            : base(options)
        {
        }

        public DbSet<AzureDotNetCoreWebAppDemo.Models.ToDoItem> ToDoItem { get; set; }
    }
}
