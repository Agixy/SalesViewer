using Microsoft.EntityFrameworkCore;
using SalesViewerService.Interfaces;
using SalesViewerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesViewerService.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly BillsDbContext _context;

        public BillRepository(BillsDbContext context)
        {
            _context = context;            
        }

        public async Task<IEnumerable<Bill>> GetBillsByDate(DateTime startDate, DateTime endDate)
        {
            return await _context.Bills.Where(b => b.OpenDate.Date >= startDate.Date && b.OpenDate.Date <= endDate.Date)
                .Include(n => n.BillsItems)
                .Include(n => n.DiscountForm)
                .Include(n => n.Table)
                .Include(n => n.Waiter).ToListAsync();
        }

        public async Task<IEnumerable<BillsItem>> GetBillItemsByBillNumber(int billNumber)
        {
            var billId =  _context.Bills.FirstOrDefault(b => b.Number == billNumber).Id;
            return await _context.BillsItems.Where(b => b.BillId == billId).Include(b => b.MenuItem).ToListAsync();               
        }
    }
}
