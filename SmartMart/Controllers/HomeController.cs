using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMart.Data;
using SmartMart.Models;
using SmartMart.ViewModel;
using System.Diagnostics;

namespace SmartMart.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext context;

public HomeController(AppDbContext context)
{
    this.context = context;
}
        public IActionResult Index()
        {
            HomeData Data = new HomeData
            {
                countProducts = context.Products.Count(),
                countCategory = context.Categories.Count(),
                countCustomers = context.Customers.Count()
            };
            return View(Data);
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
