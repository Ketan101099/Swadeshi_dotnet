using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleCRMApp.Data;
using SwadeshiApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace SwadeshiApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        string email = null;
        public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Action method to fetch user cart details
        public async Task<IActionResult> Index()
        {
            IdentityUser user = await _userManager.GetUserAsync(User);
            email = user.Email;
            var userId = email; // Get the user ID from the logged-in user
            

             var cartItems = _context.CartItem
           .Include(ci => ci.Product) // Include the Product navigation property
           .Where(ci => ci.UserId == userId)
           .ToList();

            return View(cartItems);
        }

        // Action method to remove a product from the cart
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var cartItem = _context.CartItem.Find(cartItemId);

            if (cartItem == null)
            {
                return NotFound(); // Cart item not found, handle accordingly
            }

            _context.CartItem.Remove(cartItem);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Action method to add a product to the cart

        //public async Task<IActionResult> AddToCart(int productId)
        //{
        //    IdentityUser user = await _userManager.GetUserAsync(User);
        //    email = user.Email;
        //    var userId = email;


        //    var product = _context.Product.Find(productId);

        //    if (product == null)
        //    {
        //        return NotFound(); // Product not found, handle accordingly
        //    }

        //    var cartItem = _context.CartItem.FirstOrDefault(ci => ci.ProductID == productId && ci.UserId == userId);
        //    

        //    if (cartItem != null)
        //    {
        //        cartItem.Quantity++; // Increase quantity if product already exists in cart
        //    }
        //    else
        //    {
        //        cartItem = new Cart
        //        {
        //            ProductID = productId,
        //            Quantity = 1,
        //            UserId = userId
        //        };

        //        _context.CartItem.Add(cartItem);
        //    }

        //    _context.SaveChanges();

        //    return RedirectToAction("Index");
        //}

        public async Task<IActionResult> AddToCart(int productId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // User is not logged in, redirect to the login page
                return Redirect("/Identity/Account/Login");

            }

            IdentityUser user = await _userManager.GetUserAsync(User);
            string email = user.Email;
            var userId = email;

            var product = await _context.Product.FindAsync(productId);

            if (product == null)
            {
                return NotFound(); // Product not found, handle accordingly
            }

            var cartItem = await _context.CartItem.FirstOrDefaultAsync(ci => ci.ProductID == productId && ci.UserId == userId);

            if (cartItem != null)
            {
                cartItem.Quantity++; // Increase quantity if product already exists in cart
            }
            else
            {
                cartItem = new Cart // Assuming CartItem is the correct class name
                {
                    ProductID = productId,
                    Quantity = 1,
                    UserId = userId
                };

                _context.CartItem.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}