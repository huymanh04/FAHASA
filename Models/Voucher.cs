using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public enum VoucherDiscountType
    {
        Percentage = 0, // Value stored as 0.05 (5%), 0.10 (10%)
        FixedAmount = 1 // Value stored as 10000, 50000 etc (VND)
    }

    public class Voucher
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        public VoucherDiscountType DiscountType { get; set; }

        // Percentage: 0.05 = 5% ; FixedAmount: currency value
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }

        // Minimum order subtotal required (null = no min)
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinOrderTotal { get; set; }

        // Max number of successful uses per day (0 or null = unlimited)
        public int DailyLimit { get; set; }

        // Internal tracking of uses for current day
        public int DailyUsedCount { get; set; }

        public DateTime LastResetDate { get; set; } = DateTime.UtcNow.Date;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Giới hạn số lần 1 user được dùng trong ngày (0 = không giới hạn)
        public int PerUserDailyLimit { get; set; }

        // Giới hạn số lần 1 user được dùng trong tuần (0 = không giới hạn)
        public int PerUserWeeklyLimit { get; set; }
    }
}
