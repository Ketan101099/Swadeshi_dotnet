using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwadeshiApp.Models
{
    public class Product
    {
       
        [Key]
        public int ProductID { get; set; }
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Display(Name = "Supplier")]
        public string SupplierID { get; set; }
       
      
     
      
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "Previous Price")]
        public Nullable<decimal> OldPrice { get; set; }
        public Nullable<decimal> Discount { get; set; }
        [Display(Name = "Stock")]
        public Nullable<int> UnitInStock { get; set; }
        [Display(Name = "Available?")]
        public Nullable<bool> ProductAvailable { get; set; }
        [Display(Name = "Description")]
        public string ShortDescription { get; set; }
        [Display(Name = "Picture")]
        public string PicturePath { get; set; }
        public string Note { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public string StateName { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

       
    }
}
