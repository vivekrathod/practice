using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AzureDotNetWebAppDemo.Models;

namespace AzureDotNetWebAppDemo.ViewModels
{
    public class ToDoItemViewModel
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public bool Finished { get; set; }
    }
}