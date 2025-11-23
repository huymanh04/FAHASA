using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public class Product
    {
        [Key]
        public long? ProductID { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(8, 2)")]
        [Range(0, 999999.99, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        [Range(0, 999999.99, ErrorMessage = "Rent price must be a positive number")]
        public decimal? RentPrice { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Navigation property
        public Category Category { get; set; } = null!;

        // Tác giả
        public int? AuthorId { get; set; }
        public Author? Author { get; set; }

        public string? Image { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative")]
        public int Quantity { get; set; }

        public bool IsForSale { get; set; } = true;
        public bool IsForRent { get; set; } = false;

        [Range(0, 100)]
        public int ConditionPercent { get; set; } = 100;

        [Range(1, 365, ErrorMessage = "Rent duration must be between 1 and 365 days")]
        public int? RentDurationDays { get; set; }
        public List<ProductImage> ProductImages { get; set; } = new();
        public ICollection<ProductReview>? Reviews { get; set; }

        // Sale fields
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? SalePrice { get; set; }
        
        public DateTime? SaleStartTime { get; set; }
        
        public DateTime? SaleEndTime { get; set; }

        // Helper property to check if product is on sale
        public bool IsOnSale => SalePrice.HasValue && 
                                SaleStartTime.HasValue && 
                                SaleEndTime.HasValue &&
                                DateTime.Now >= SaleStartTime.Value && 
                                DateTime.Now <= SaleEndTime.Value;

        // Get current price (sale or regular)
        public decimal CurrentPrice => IsOnSale && SalePrice.HasValue ? SalePrice.Value : Price;
    }
}
