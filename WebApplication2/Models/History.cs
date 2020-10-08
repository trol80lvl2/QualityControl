using System;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public partial class History
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public DateTime? Date { get; set; }
        public string TaskNum { get; set; }
        public string? Year { get; set; }
        public string Status { get; set; }
        public string Wuser { get; set; }
    }
}
