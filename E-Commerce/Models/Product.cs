using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class Product
    {
        
        [Key]
        [Required(ErrorMessage = "Product Code is required")]
        public string Code { get; set; }


        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(50, ErrorMessage = "Product Name must be between 3 and 50 characters", MinimumLength = 3)]
        public string Name { get; set; }


        [Required(ErrorMessage = "Product Description is required")]
        [StringLength(500, ErrorMessage = "Product Description must be between 3 and 500 characters", MinimumLength = 3)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0,1000000, ErrorMessage = "Price must be a positive number")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }


        [Required(ErrorMessage = "Category is required")]
        [StringLength(50, ErrorMessage = "Category must be between 3 and 50 characters", MinimumLength = 3)]
        public string Category { get; set; }


        [Required(ErrorMessage = "Minimum Quantity is required")]
        public int MinimumQuantity { get; set; }


        [Required(ErrorMessage = "Discount Rate is required")]
        [Range(0, 1000000, ErrorMessage = "Discount Rate must be a positive number")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal DiscountRate { get; set; }

    }
}
