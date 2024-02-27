using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleCRMApp.Data;
using SwadeshiApp.Models; // Assuming you have a Product model

namespace SwadeshiApp.Controllers
{
    public class ProductViewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductViewController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult GetProduct(string stateName)
        {
            // Assuming you have a Product model with a StateName property
            var products = _context.Product
               
                .Where(p => p.StateName == stateName)
                .ToList();

            return View(products);
        }
    }
}
