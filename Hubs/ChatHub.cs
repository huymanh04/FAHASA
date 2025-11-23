using Microsoft.AspNetCore.SignalR;
using SportsStore.Models;
using System.Threading.Tasks;
using System;

namespace SportsStore.Hubs
{
    public class ChatHub : Hub
    {
        private readonly StoreDbContext _context;

        public ChatHub(StoreDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string userId, string userName, string message, bool isAdmin)
        {
            // Broadcast message to connected clients always so admin can see live messages.
            var time = DateTime.Now.ToString("HH:mm");
            await Clients.All.SendAsync("ReceiveMessage", userId ?? string.Empty, userName ?? string.Empty, message ?? string.Empty, isAdmin, time);

            // Persist message only when sender is authenticated (has a userId) or is an admin.
            if (isAdmin || !string.IsNullOrEmpty(userId))
            {
                var chatMessage = new ChatMessage
                {
                    UserId = userId ?? string.Empty,
                    UserName = userName ?? string.Empty,
                    Message = message ?? string.Empty,
                    IsFromAdmin = isAdmin,
                    SentAt = DateTime.Now
                };

                _context.ChatMessages.Add(chatMessage);
                await _context.SaveChangesAsync();
            }
        }
    }
}

