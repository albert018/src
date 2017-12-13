using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models
{
    [MetadataType(typeof(CategoryMeta))]
    public partial class Category { }

    public class CategoryMeta
    {
        [Display(Name = "Category ID")]
        public int ID { get; set; }
        [Display(Name = "Category Name")]
        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}