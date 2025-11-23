using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    /// <summary>
    /// Lưu trữ giỏ hàng của user vào database để persistent qua các session
    /// </summary>
    public class UserCartItem
    {
        [Key]
        public int UserCartItemId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public long ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public bool IsRental { get; set; } = false;

        public int RentalDays { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}
