using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public partial class Defects
    {
        [Key]
        public int DefectId { get; set; }
        public string Nameua { get; set; }
        public string Nameit { get; set; }
        public string Nameen { get; set; }
    }
}
