using SportsStore.Models;

namespace SportsStore.Services
{
    public interface INotificationService
    {
        Task CreateOrderNotificationAsync(string userId, int orderId, string status, string message);
        Task<List<Notification>> GetUserNotificationsAsync(string userId, int count = 10);
        Task<int> GetUnreadCountAsync(string userId);
        Task MarkAsReadAsync(int notificationId);
        Task MarkAllAsReadAsync(string userId);
    }
}
