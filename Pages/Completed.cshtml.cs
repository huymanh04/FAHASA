using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Pages
{
    public class CompletedModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;

        public CompletedModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Order? CompletedOrder { get; set; }

        public void OnGet(int? orderId)
        {
            if (orderId.HasValue)
            {
                CompletedOrder = _orderRepository.Orders
                    .FirstOrDefault(o => o.OrderID == orderId.Value);
            }
        }
    }
}
