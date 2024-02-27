using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SaleCRMApp.Models
{
    
        public class State
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "State Name is required")]
            public string StateName { get; set; }

            [Required(ErrorMessage = "State Description is required")]
            public string StateDescription { get; set; }

            public byte[] StateImage { get; set; }

            [NotMapped]
            public IFormFile StateImageFile { get; set; }
        }
  
}
