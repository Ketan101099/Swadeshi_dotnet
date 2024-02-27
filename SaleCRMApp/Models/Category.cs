
using System.ComponentModel.DataAnnotations;

namespace SwadeshiApp.Models
{
    public class Category
    {


        public Category()
        {

            this.Products = new HashSet<Product>();
            this.SubCategories = new HashSet<SubCategory>();
        }

        [Key]
        public int CategoryID { get; set; }
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public Nullable<bool> isActive { get; set; }


        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}