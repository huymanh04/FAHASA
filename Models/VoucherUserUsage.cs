using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public class VoucherUserUsage
    {
        [Key]
        public int Id { get; set; }
        public int VoucherId { get; set; }
        [Required]
        [MaxLength(450)] // matches typical AspNet Identity user id length
        public string UserId { get; set; } = string.Empty;
        public DateTime PeriodStartDate { get; set; }
        // "Daily" hoặc "Weekly"
        [Required]
        [MaxLength(20)]
        public string PeriodType { get; set; } = "Daily";
        public int UsageCount { get; set; }

        [ForeignKey(nameof(VoucherId))]
        public Voucher Voucher { get; set; } = null!;
    }
}
