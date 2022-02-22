using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalesViewer.Models;
using SalesViewerService.Interfaces;
using SalesViewerService.Models;

namespace SalesViewer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBillRepository billRepository;     
        public IConfiguration Configuration { get; }

        public HomeController(ILogger<HomeController> logger, IBillRepository billRepository, IConfiguration configuration)
        {
            this.billRepository = billRepository;          
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
                return GetBills(DateTime.Now, DateTime.Now);
            }

            return View("Index");
        }

        [HttpPost]
        public IActionResult GetBills(DateTime startDate, DateTime endDate)
        {
            var bills = billRepository.GetBillsByDate(startDate, endDate).Result.ToList();

            var result = new BillsListViewModel() { StartDate = startDate, EndDate = endDate };

            AddBillsToViewModelList(bills, result);

            return View("BillsView", result);
        }

        
        private BillsListViewModel AddBillsToViewModelList(IList<Bill> bills, BillsListViewModel viewModelList) // TODO: Extract to separate class
        {
            foreach (var bill in bills)
            {
                decimal nettoValueOfBill = 0;
                decimal bruttoValueOfBill = 0;

                foreach (var billsItem in bill.BillsItems)
                {
                    if (billsItem.CancellationDate == null)
                    {
                        nettoValueOfBill += billsItem.NettoPrice * billsItem.Count;
                        bruttoValueOfBill += billsItem.BruttoPrice * billsItem.Count;
                    }
                }

                viewModelList.Bills.Add(new BillViewModel()   // TODO: Use AutoMapper
                {
                    Number = bill.Number,
                    DiscountType = bill.DiscountForm?.Name,
                    TableName = bill.Table.Name,
                    Waiter = bill.Waiter.ToString(),  // TODO: remove "ToString" from model
                    OpenDate = bill.OpenDate,
                    CloseDate = bill.CloseDate,
                    LastServiceDate = bill.LastServiceDate,
                    GuestsCount = (int?)bill.GuestsCount,
                    Description = bill.Description,
                    NettoValueOfBill = nettoValueOfBill,
                    BruttoValueOfBill = bruttoValueOfBill,
                    SettedValueOfBill = bill.SettedValueOfBill
                });
            }

            return viewModelList;
        }


        [HttpGet]
        public JsonResult GetMenuItems(int billNumber)
        {
            var billItems = billRepository.GetBillItemsByBillNumber(billNumber).Result;
            var menuItems = new List<string>();

            foreach (var billItem in billItems)
            {
                string cancellation = String.Empty;
                if (billItem.CancellationDate != null)
                {
                    cancellation = "ANULACJA";
                }
                menuItems.Add(billItem.MenuItem.Name + " - " + (int)billItem.Count + " sztuk " + cancellation);
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
