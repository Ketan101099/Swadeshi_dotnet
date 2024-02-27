using Microsoft.AspNetCore.Identity;


namespace SaleCRMApp.Models
{
    public class ApplicationUser : IdentityUser
    {
       public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string city { get; set; }
        public string PinCode { get; set; }

    }
}
