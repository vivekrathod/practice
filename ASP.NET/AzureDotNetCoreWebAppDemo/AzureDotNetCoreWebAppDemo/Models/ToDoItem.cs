using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AzureDotNetCoreWebAppDemo.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Finished { get; set; }
        public IdentityUser User { get; set; }
    }
}
