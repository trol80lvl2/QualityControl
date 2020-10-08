using System;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public partial class DefId
    {
        public int Id { get; set; }
        public int? IdCheck { get; set; }
        public int? DefectId { get; set; }
        public string Comment { get; set; }
        public string Def { get; set; }
        public Defects Defect { get; set; }
    }
}
