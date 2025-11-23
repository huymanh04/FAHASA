using System;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        public string? Link { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string NotificationType { get; set; } = "Order"; // Order, System, Promotion, etc.

        public int? OrderId { get; set; }
    }
}
