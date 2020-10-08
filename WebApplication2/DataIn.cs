using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2
{
    public static class DataIn
    {
        public static void Initialize(OrderContext context)
        {
            if (context.Checks.Any())
            {
                context.Checks.AddRange(
                    new Check
                    {
                        Date = new DateTime(2019,12,05),
                        Articolo = "06dasdad",
                        Color = "sdadasdasdasd",
                        TaskNum="90sdsadasdasdsa48/1",
                        Client="sdasdasdSunkla",
                        Total=71,
                        Selection=7,
                        Machine="7mb",
                        MaxDef="",
                        MinDef="Неутdsaddяжка клей на стельке\n№1-36р-ок\n№2-42р-ок",
                        
                    }

                    );

                context.SaveChanges();

            }
        }

    }
}
