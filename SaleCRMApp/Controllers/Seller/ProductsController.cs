using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SaleCRMApp.Data;
using SaleCRMApp.Models;
using SwadeshiApp.Models;

namespace SwadeshiApp.Controllers.Seller
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {


            IdentityUser user = await _userManager.GetUserAsync(User);
            string email = user.Email;
           
            if (user == null)
            {
                
                return RedirectToAction("Login", "Account");
            }
            
            // Fetch products associated with the current user
            var products = await _context.Product
                .Where(p => p.SupplierID == email)
                .ToListAsync();

            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product

                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> ViewProduct(int? ProductID)
        {
            if (ProductID == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product

                .FirstOrDefaultAsync(m => m.ProductID == ProductID);
            if (product == null)
            {
                return NotFound();
            }

            return View("ViewProductDetails",product);
        }
        public async Task<IActionResult> ViewProductDetails(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product

                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View("ViewProductDetails",product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            //ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID");
            //ViewData["SubCategoryID"] = new SelectList(_context.SubCategory, "SubCategoryID", "SubCategoryID");
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "Address");
            ViewData["StateName"] = new SelectList(_context.States.Select(c => c.StateName).Distinct());
            ViewData["CategoryName"] = new SelectList(_context.Category.Select(c => c.CategoryName).Distinct());
            ViewData["SubCategoryName"] = new SelectList(_context.SubCategory.Select(c => c.SubCategoryName).Distinct());

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,Name,SupplierID,StateName,CategoryName,SubCategoryName,UnitPrice,OldPrice,Discount,UnitInStock,ProductAvailable,ShortDescription,PicturePath,Note,Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.PicturePath = SaveImage(product.Image);
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "Address");
            ViewData["StateName"] = new SelectList(_context.States.Select(c => c.StateName).Distinct());
            ViewData["CategoryName"] = new SelectList(_context.Category.Select(c => c.CategoryName).Distinct());
            ViewData["SubCategoryName"] = new SelectList(_context.SubCategory.Select(c => c.SubCategoryName).Distinct());
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "Address");
            ViewData["StateName"] = new SelectList(_context.States.Select(c => c.StateName).Distinct());
            ViewData["CategoryName"] = new SelectList(_context.Category.Select(c => c.CategoryName).Distinct());
            ViewData["SubCategoryName"] = new SelectList(_context.SubCategory.Select(c => c.SubCategoryName).Distinct());
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Name,SupplierID,StateName,CategoryName,SubCategoryName,UnitPrice,OldPrice,Discount,UnitInStock,ProductAvailable,ShortDescription,PicturePath,Note,Image")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    if (product.Image != null)                    {                        product.PicturePath = SaveImage(product.Image);                    }                    _context.Update(product);
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "Address");
            ViewData["StateName"] = new SelectList(_context.States.Select(c => c.StateName).Distinct());
            ViewData["CategoryName"] = new SelectList(_context.Category.Select(c => c.CategoryName).Distinct());
            ViewData["SubCategoryName"] = new SelectList(_context.SubCategory.Select(c => c.SubCategoryName).Distinct());
            return View(product);
        }

        // GET: Products/Delete/5
        //adding new line here changes by ketan ajinkya
        //adding new line here changes by ketan darekar
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product

                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Product?.Any(e => e.ProductID == id)).GetValueOrDefault();
        }

        private string SaveImage(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

                return "/images/" + uniqueFileName;
            }

            return null;
        }
    }
}