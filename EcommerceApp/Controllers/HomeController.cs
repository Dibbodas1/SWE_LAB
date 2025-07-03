using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Data;
using EcommerceApp.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EcommerceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get featured products - newest products first, limited to 6
            var featuredProducts = await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.Id) // Assuming Id increments with newer products
                .Take(6)
                .ToListAsync();

            // Get all categories for the navigation dropdown
            var categories = await _context.Categories.ToListAsync();
            ViewData["Categories"] = categories;
            
            // Get total product count
            var productCount = await _context.Products.CountAsync();
            ViewData["ProductCount"] = productCount;
            
            // Get category count
            var categoryCount = categories.Count;
            ViewData["CategoryCount"] = categoryCount;
            
            return View(featuredProducts);
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

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}