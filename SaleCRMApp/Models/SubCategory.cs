
using System.ComponentModel.DataAnnotations;

namespace SwadeshiApp.Models
{
    public class SubCategory
    {
        public SubCategory()
        {
            this.Products = new HashSet<Product>();
        }

        [Key]
        public int SubCategoryID { get; set; }
     

        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string Description { get; set; }
        public Nullable<bool> isActive { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
