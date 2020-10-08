using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class OrderCreateViewModel
    {
        public DateTime Date { get; set; }
        public string Articolo { get; set; }
        public string Color { get; set; }
        public string TaskNum { get; set; }
        public string Client { get; set; }
        public int Total { get; set; }
        public int Selection { get; set; }
        public string Machine { get; set; }
        public string MaxDef { get; set; }

        public string MinDef { get; set; }
        public bool CanGo { get; set; }
        public string? Stagione { get; set; }
        public string? Linea { get; set; }
        public string? Fustella { get; set; }
        public List<IFormFile> Photo { get; set; }

        public User User { get; set; }
    }
}
