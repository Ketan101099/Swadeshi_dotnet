using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using SaleCRMApp.Data;
using SaleCRMApp.Models;
using SwadeshiApp.DTO;
using SwadeshiApp.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwadeshiApp.Controllers.User
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private OrderItem _orderDetails; // Define _orderDetails as a class-level variable

        public PaymentController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(decimal TotalAmount, int ProductId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // User is not logged in, redirect to the login page
                return Redirect("/Identity/Account/Login");

            }
            IdentityUser user = await _userManager.GetUserAsync(User);
            string email = user?.Email;

            if (user != null)
            {
                UserEntity userEntity = new UserEntity(); // Replace with the appropriate class
                UserProfile userDetail = userEntity.GetUserDetail(email);
                userDetail.Email=email;

                var viewModel = new IndexViewModel
                {
                    UserDetail = userDetail,
                    OrderDetails = new OrderItem
                    {
                        ProductId = ProductId,
                        TotalAmount = TotalAmount
                    }
                };

                // Set _orderDetails
                _orderDetails = viewModel.OrderDetails;

                return View("OrderDetails", viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        //[HttpPost]
        //public IActionResult initiateOrder(string UserId,string FirstName,string LastName,string MobileNo,string Address,string city,string PinCode,decimal TotalAmount,int ProductId)
        //{
        //    string key = "rzp_test_1gpJJoIcng5ogt";
        //    string secret = "2EfZzlT8kFEJGTVVWfDfyZB5";

        //    OrderItem orderdetails = new OrderItem();

        //    orderdetails.FirstName = FirstName;
        //    orderdetails.LastName = LastName;
        //    orderdetails.UserId = UserId;
        //    orderdetails.ProductId = ProductId;
        //    orderdetails.MobileNo = MobileNo;
        //    orderdetails.Address = Address;
        //    orderdetails.city = city;
        //    orderdetails.PinCode = PinCode;
        //    orderdetails.TotalAmount = TotalAmount;

        //    Random _random = new Random();
        //    string TransactionId = _random.Next(0, 10000).ToString();

        //    Dictionary<string, object> input = new Dictionary<string, object>();

        //    // Use _orderDetails instead of _orderDetails.TotalAmount
        //    input.Add("amount", Convert.ToDecimal(_orderDetails.TotalAmount) * 100);
        //    input.Add("currency", "INR");
        //    input.Add("receipt", TransactionId);

        //    RazorpayClient client = new RazorpayClient(key, secret);
        //    Razorpay.Api.Order order = client.Order.Create(input);

        //    ViewBag.orderid = order["id"].ToString();

        //    return View("Payment", _orderDetails);
        //}
        [HttpPost]
        public IActionResult initiateOrder(IndexViewModel viewModel)
        {
            string key = "rzp_test_1gpJJoIcng5ogt";
            string secret = "2EfZzlT8kFEJGTVVWfDfyZB5";

            // Access the order details from the viewModel
            OrderItem orderdetails = viewModel.OrderDetails;
            orderdetails.LastName = viewModel.UserDetail.LastName;
            orderdetails.FirstName=viewModel.UserDetail.FirstName;
            orderdetails.MobileNo= viewModel.UserDetail.MobileNo;
            orderdetails.UserId = viewModel.UserDetail.Email;
            Random _random = new Random();
            string TransactionId = _random.Next(0, 10000).ToString();

            Dictionary<string, object> input = new Dictionary<string, object>();

            input.Add("amount", Convert.ToDecimal(orderdetails.TotalAmount) * 100);
            input.Add("currency", "INR");
            input.Add("receipt", TransactionId);

            RazorpayClient client = new RazorpayClient(key, secret);
            Razorpay.Api.Order order = client.Order.Create(input);

            ViewBag.orderid = order["id"].ToString();

            return View("Payment", orderdetails);
        }


        public IActionResult Payment(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature, string FirstName, string LastName, string MobileNo,
            string Address, string city, string PinCode, int ProductId, decimal TotalAmount, string UserId)
        {
            Dictionary<string, string> attribute = new Dictionary<string, string>();
            attribute.Add("razorpay_payment_id", razorpay_payment_id);
            attribute.Add("razorpay_order_id", razorpay_order_id);
            attribute.Add("razorpay_signature", razorpay_signature);

            Utils.verifyPaymentSignature(attribute);

            OrderItem orderdetails = new OrderItem();
            orderdetails.PaymentId = razorpay_payment_id;
            orderdetails.FirstName = FirstName;
            orderdetails.LastName = LastName;
            orderdetails.UserId = UserId;
            orderdetails.ProductId = ProductId;
            orderdetails.MobileNo = MobileNo;
            orderdetails.Address = Address;
            orderdetails.City = city;
            orderdetails.PinCode = PinCode;
            orderdetails.TotalAmount = TotalAmount;
            orderdetails.OrderDate = DateTime.Now;
            orderdetails.PaymentMode = "Online";

            return RedirectToAction("CreateOrder", "Order", orderdetails);
        }
    }
}
