using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleCRMApp.Data;
using SwadeshiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwadeshiApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
       
        public OrderController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Create order

        //public async Task<IActionResult> CreateOrder(OrderItem orderItem)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        orderItem.Product = await _context.Product.FindAsync(orderItem.ProductId);
        //        orderItem.Email = orderItem.UserId;
        //        orderItem.TotalAmount = orderItem.Product.UnitPrice;
        //        orderItem.SupplierId = orderItem.Product.SupplierID;
        //        orderItem.OrderStatus = "Ordered";
        //        _context.Add(orderItem);
        //        await _context.SaveChangesAsync();

        //    }

        //    return RedirectToAction(nameof(OrdersByUserId));
        //}

        public async Task<IActionResult> CreateOrder(OrderItem orderItem)
        {
            if (!ModelState.IsValid)
            {
                orderItem.Product = await _context.Product.FindAsync(orderItem.ProductId);
                orderItem.Email = orderItem.UserId;
                orderItem.TotalAmount = orderItem.Product.UnitPrice;
                orderItem.SupplierId = orderItem.Product.SupplierID;
                orderItem.OrderStatus = "Ordered";
                _context.Add(orderItem);

                // Decrease stock by 1
                if (orderItem.Product.UnitInStock > 0)
                {

                    orderItem.Product.UnitInStock -= 1;

                    if (orderItem.Product.UnitInStock <= 0)
                    {
                        orderItem.Product.ProductAvailable = false;
                    }
                }

                try
                {
                    _context.Update(orderItem.Product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(orderItem.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(OrdersByUserId));
        }

       


        //public async Task<IActionResult> CreateCOD([Bind("FirstName,LastName,MobileNo,Email,TotalAmount,Address,City,PinCode,UserId,ProductId")]OrderItem orderItem)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        orderItem.Product = await _context.Product.FindAsync(orderItem.ProductId);
        //        orderItem.TotalAmount = orderItem.Product.UnitPrice;
        //        orderItem.UserId = orderItem.Email;

        //        orderItem.SupplierId = orderItem.Product.SupplierID;
        //        orderItem.PaymentMode = "Cash On Delivery";
        //        orderItem.OrderDate = DateTime.Now;
        //        orderItem.OrderStatus = "Ordered";
        //        _context.Add(orderItem);
        //        await _context.SaveChangesAsync();

        //    }

        //    return RedirectToAction(nameof(OrdersByUserId));
        //}
        [HttpPost]
        public async Task<IActionResult> CreateCOD([Bind("FirstName,LastName,MobileNo,Email,TotalAmount,Address,City,PinCode,UserId,ProductId")] OrderItem orderItem)
        {
            if (!ModelState.IsValid)
            {
                orderItem.Product = await _context.Product.FindAsync(orderItem.ProductId);
                orderItem.TotalAmount = orderItem.Product.UnitPrice;
                orderItem.UserId = orderItem.Email;

                orderItem.SupplierId = orderItem.Product.SupplierID;
                orderItem.PaymentMode = "Cash On Delivery";
                orderItem.OrderDate = DateTime.Now;
                orderItem.OrderStatus = "Ordered";
                _context.Add(orderItem);

                // Decrease stock by 1
                if(orderItem.Product.UnitInStock > 0) {

                    orderItem.Product.UnitInStock -= 1;

                    if(orderItem.Product.UnitInStock <= 0)
                    {
                        orderItem.Product.ProductAvailable =false;
                    }
                }
               

                try
                {
                    _context.Update(orderItem.Product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(orderItem.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(OrdersByUserId));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }

        // Fetch orders by userId
        public async Task<IActionResult> OrdersByUserId()
        {
            string userId;
            IdentityUser user = await _userManager.GetUserAsync(User);
            userId = user.Email;
            //var orders = await _context.OrderItem.Where(o => o.UserId == userId).ToListAsync();
            var orders = await _context.OrderItem
            .Include(o => o.Product) // Include the Product details
            .Where(o => o.UserId == userId)
            .ToListAsync();

            return View("Orders", orders);
        }

        public async Task<IActionResult> NewOrderBySellerId()
        {
            string userId;
            IdentityUser user = await _userManager.GetUserAsync(User);
            userId = user.Email;
            //var orders = await _context.OrderItem.Where(o => o.UserId == userId).ToListAsync();
            var orders = await _context.OrderItem
            .Include(o => o.Product) // Include the Product details
            .Where(o => o.SupplierId == userId && o.OrderStatus == "Ordered")
            .ToListAsync();

            return View("SellerOrders", orders);
        }
        public async Task<IActionResult> ShippedOrderBySellerId()
        {
            string userId;
            IdentityUser user = await _userManager.GetUserAsync(User);
            userId = user.Email;
            //var orders = await _context.OrderItem.Where(o => o.UserId == userId).ToListAsync();
            var orders = await _context.OrderItem
            .Include(o => o.Product) // Include the Product details
            .Where(o => o.SupplierId == userId && o.OrderStatus == "Shipped")
            .ToListAsync();

            return View("SellerOrders", orders);
        }
        public async Task<IActionResult> DeliveredOrderBySellerId()
        {
            string userId;
            IdentityUser user = await _userManager.GetUserAsync(User);
            userId = user.Email;
            //var orders = await _context.OrderItem.Where(o => o.UserId == userId).ToListAsync();
            var orders = await _context.OrderItem
            .Include(o => o.Product) // Include the Product details
            .Where(o => o.SupplierId == userId && o.OrderStatus == "Delivered")
            .ToListAsync();

            return View("SellerOrders", orders);
        }

        // Edit order by orderId
        public async Task<IActionResult> Edit(int? orderId)
        {
            if (orderId == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem.FindAsync(orderId);
            if (orderItem == null)
            {
                return NotFound();
            }
            return View(orderItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int orderId, OrderItem orderItem)
        {
            if (orderId != orderItem.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (orderItem.OrderId.HasValue && OrderItemExists(orderItem.OrderId.Value))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(OrdersByUserId));
            }
            return View(orderItem);
        }

        // Delete order by orderId
        public async Task<IActionResult> Delete(int? orderId)
        {
            if (orderId == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem
          .Include(m => m.Product) 
          .FirstOrDefaultAsync(m => m.OrderId == orderId);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View("DeleteConfirm",orderItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int orderId)
        {
            var orderItem = await _context.OrderItem.FindAsync(orderId);
            _context.OrderItem.Remove(orderItem);
            await _context.SaveChangesAsync();
            //orderItem.OrderStatus = "Cancelled";
            //orderItem.CancelledDate = DateTime.Now;
            //_context.OrderItem.Update(orderItem);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(OrdersByUserId));
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItem.Any(e => e.OrderId == id);
        }

        public async Task<IActionResult> FetchOrderDetails(int orderId)
        {
            var orderItem = await _context.OrderItem
                .Include(o => o.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);


            return View("ViewOrderDetails",orderItem);
        }

        public IActionResult ViewOrderDetails(int orderId)
        {
            return PartialView("_OrderDetails");
        }

        public async Task<IActionResult> OrderShipement(int orderId)
        {
            var orderItem = await _context.OrderItem.FindAsync(orderId);
           
            orderItem.OrderStatus = "Shipped";
            orderItem.ShippingDate = DateTime.Now;
            _context.OrderItem.Update(orderItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(NewOrderBySellerId));
        }

        public async Task<IActionResult> OrderDeliverd(int orderId)
        {
            var orderItem = await _context.OrderItem.FindAsync(orderId);

            orderItem.OrderStatus = "Delivered";
            orderItem.DeliveryDate = DateTime.Now;
            _context.OrderItem.Update(orderItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShippedOrderBySellerId));
        }



    }
}
