using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Check
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Articolo { get; set; }
        public string Color { get; set; }
        public string TaskNum { get; set; }
        public string Client { get; set; }
        public int Total { get; set; }
        public int Selection { get; set; }
        public string Machine { get; set; }
        public string MaxDef { get; set; }
        public string UserId { get; set; }
        public string MinDef { get; set; }
        public string Status { get; set; }
        public DateTime? Date_user { get; set; }
        public DateTime? Date_checked { get; set; }
        public string WhoChecked { get; set; }
        public string? Commentary { get; set; }
        public char? Defect { get; set; }
        public string? Stagione { get; set; }
        public string? Linea { get; set; }
        public string? Fustella { get; set; }
        public Check() { }
        public Check(DateTime DateC, string ArticoloC, string ColorC, string TaskNumC, string ClientC, short TotalC, byte SelectionC, string MachineC, string MaxDefC, string MinDefC)
        {
            Date = DateC;
            Articolo = ArticoloC;
            Color = ColorC;
            TaskNum = TaskNumC;
            Client = ClientC;
            Total = TotalC;
            Selection = SelectionC;
            Machine = MachineC;
            MaxDef = MaxDefC;
            MinDef = MinDefC;
        }
    }
}
