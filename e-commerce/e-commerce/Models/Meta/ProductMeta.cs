using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.Models
{
    [MetadataType(typeof(ProductMeta))]
    public partial class Product { }

    public class ProductMeta
    {
        [Display(Name = "Product ID")]
        public int ID { get; set; }
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Display(Name = "Product Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Product Price")]
        [Range(0,5000,ErrorMessage ="please enter a number between 0 to 5000")]
        [RegularExpression(@"[0-9]+(\.[0-9][0-9]?)",ErrorMessage ="must be a number")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString ="{0:c}")]
        public decimal Price { get; set; }
        [Display(Name = "Product Category ID")]
        public int? CategoryID { get; set; }
        [ForeignKey(nameof(CategoryID))]
        public virtual Category Category { get; set; }
    }
}