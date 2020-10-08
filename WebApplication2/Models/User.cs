using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class User
    {

        public int Id { get; set; }
        public string Name { get; set; }
     [Key]
        public int Id_zap { get; set; }
    }
}
