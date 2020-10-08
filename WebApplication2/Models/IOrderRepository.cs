using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    interface IOrderRepository
    {
        Check GetCheck(int Id);
        IEnumerable<Check> GetAllChecks();
        Check Add(Check check);
        Check Update(Check checkChanges);
        Check Delete(int id);
    }
}
