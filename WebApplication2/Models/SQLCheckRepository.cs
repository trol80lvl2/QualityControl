using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class SQLCheckRepository : IOrderRepository
    {
        private readonly OrderContext context;
        public SQLCheckRepository(OrderContext context)
        {

        }
        public Check Add(Check check)
        {
            context.Checks.Add(check);
            context.SaveChanges();
            return check;
        }

        public Check Delete(int id)
        {
            Check check = context.Checks.Find(id);
            if (check != null)
            {
                context.Checks.Remove(check);
                context.SaveChanges();
            }
            return check;
        }

        public IEnumerable<Check> GetAllChecks()
        {
            return context.Checks;
        }

        public Check GetCheck(int Id)
        {
            return context.Checks.Find(Id);
        }

        public Check Update(Check checkChanges)
        {
            var check = context.Checks.Attach(checkChanges);
            check.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return checkChanges;
        }
    }
}
