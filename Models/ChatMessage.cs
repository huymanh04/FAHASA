using System;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public string UserName { get; set; } = string.Empty;
        
        [Required]
        public string Message { get; set; } = string.Empty;
        
        public DateTime SentAt { get; set; } = DateTime.Now;
        
        public bool IsFromAdmin { get; set; } = false;
        
        public bool IsRead { get; set; } = false;
    }
}

