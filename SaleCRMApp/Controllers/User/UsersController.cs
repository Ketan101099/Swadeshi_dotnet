using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SaleCRMApp.Data;
using SaleCRMApp.Models;
using SwadeshiApp.DTO;
using System.Threading.Tasks;

namespace SwadeshiApp.Controllers.User
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        string email = null;
        public UsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IdentityUser user = await _userManager.GetUserAsync(User);
            email = user.Email;

            if (user != null)
            {
                UserEntity userEntity = new UserEntity(); // Replace with the appropriate class
                UserProfile userDetail = userEntity.GetUserDetail(email);

                return View(userDetail);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Sellers()
        {
           

                UserEntity userEntity = new UserEntity(); // Replace with the appropriate class
               List<UserProfile> sellerlist= userEntity.GetSellerList();

                return View("SellerList",sellerlist);
           
        }

        [HttpGet]
        public IActionResult Edit()
        {
            // You should handle user authentication here before proceeding to edit.

            UserEntity userEntity = new UserEntity(); // Replace with the appropriate class
            UserProfile user = userEntity.GetUserDetail(User.Identity.Name);

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(UserProfile user)
        {
            // Validate and save changes to the user profile

             UserEntity userEntity = new UserEntity(); // Uncomment this line if needed
             UserProfile updatedUser = userEntity.SaveUserDetail(User.Identity.Name, user);

            // Redirect to the appropriate action or view
            return RedirectToAction("Index");
        }
    }
}
