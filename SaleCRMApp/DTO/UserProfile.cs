using System.ComponentModel.DataAnnotations;

namespace SwadeshiApp.DTO
{
    public class UserProfile
    {
        [Key]
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string city { get; set; }
        public string PinCode { get; set; }

    }
}
