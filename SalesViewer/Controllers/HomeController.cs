using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalesViewer.Models;
using SalesViewerService;

namespace SalesViewer.Controllers
{
    public class HomeController : Controller
    {
        private readonly BillsDbContext _context;
        public IConfiguration Configuration { get; }

        public HomeController(ILogger<HomeController> logger, BillsDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckPassword(string password)
        {
            if(password.Equals(Configuration.GetSection("Passwords")["MainAccess"]))
            {
                return View("BillsView");
            }

            return View("Index");
        }

        [HttpPost]
        public IActionResult GetBills(DateTime date1, DateTime date2)
        {
            var bills = _context.Bills.Where(b => b.OpenDate.Date >= date1.Date && b.OpenDate.Date <= date2.Date).ToList();
            var result = new List<BillViewModel>();

            foreach (var item in bills)
            {
                string discount = "-";
                if (item.DiscountFormId != null)
                {
                    var discForm = _context.DiscountForms.FirstOrDefault(d => d.Id == item.DiscountFormId);
                    
                    if (discForm != null)
                    {
                        discount = discForm.Name + " - " + _context.DiscountTypes.FirstOrDefault(d => d.Id == discForm.DiscountTypeId).Name;
                    }
                }

                var items = _context.BillsItems.Where(i => i.BillsId == item.Id).ToList();

                decimal nettoValueOfBill = 0;
                decimal bruttoValueOfBill = 0;
                DateTime? date;

                foreach (var menuItem in items)
                {
                    if(menuItem.CancellationDate == null)
                    {
                        nettoValueOfBill += menuItem.NettoPrice * menuItem.Count;
                        bruttoValueOfBill += menuItem.BruttoPrice * menuItem.Count;
                    }          
                }
                

                result.Add(new BillViewModel()   // TODO: Use AutoMapper
                {
                    Number = item.Number,
                    DiscountType = discount, 
                    TableName = _context.Tables.FirstOrDefault(t => t.Id == (item.TableId ?? 0))?.Name,
                    Waiter = _context.Waiters.FirstOrDefault(w => w.Id == item.WaiterId).ToString(),  // TODO: remove "ToString" from model
                    OpenDate = item.OpenDate,
                    CloseDate = item.CloseDate,
                    LastServiceDate = item.LastServiceDate,
                    GuestsCount = (int?)item.GuestsCount,
                    Description = item.Description,
                    NettoValueOfBill = nettoValueOfBill,
                    BruttoValueOfBill = bruttoValueOfBill,
                    SettedValueOfBill = item.SettedValueOfBill
                });
            }

            return View("BillsView", result);
        }

        [HttpGet]
        public JsonResult GetMenuItems(int billNumber)
        {
            var id = _context.Bills.FirstOrDefault(b => b.Number == billNumber).Id;
            var billsItems = _context.BillsItems.Where(i => i.BillsId == id).ToList();
            var menuItems = new List<string>();

            foreach (var billItem in billsItems)
            {
                string cancellation = String.Empty;
                if (billItem.CancellationDate != null)
                {
                    cancellation = "ANULACJA";
                }
                var menuItem = _context.MenuItems.FirstOrDefault(i => i.Id == billItem.MenuItemId);
                menuItems.Add(menuItem.Name + " - " + (int)billItem.Count + " sztuk " + cancellation);
            }
           
            return Json(menuItems);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
