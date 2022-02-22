using SalesViewerService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesViewerService.Interfaces
{
    public interface IBillRepository
    {
        Task<IEnumerable<Bill>> GetBillsByDate(DateTime startDate, DateTime endDate);
        Task<IEnumerable<BillsItem>> GetBillItemsByBillNumber(int billNumber);
    }
}