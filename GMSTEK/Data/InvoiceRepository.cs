using GMSTEK.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMSTEK.Data
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {

        public InvoiceRepository(ApplicationDBContext context)
            : base(context)
        {
        }

        public IEnumerable<Invoice> GetById(int id)
        {
            return context.Set<Invoice>().Where(q => q.InvoiceId == id)
                                  .Include(q => q.InvoiceItems)
                                  .ThenInclude(r => r.Item);
        }
    }
}
