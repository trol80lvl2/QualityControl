using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public partial class Photos
    {
        [Key]
        public int IdOrd { get; set; }
        public string PhotoPath { get; set; }
    }
}
