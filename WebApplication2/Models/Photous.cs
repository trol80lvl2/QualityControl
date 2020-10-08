using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public partial class Photous
    {
        [Key]
        public int Id { get; set; }
        public string Photo { get; set; }
    }
}
