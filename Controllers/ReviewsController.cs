using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportsStore.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace SportsStore.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly StoreDbContext _context;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(StoreDbContext context, ILogger<ReviewsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> MyReviews()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var reviews = await _context.ProductReviews
                .Include(r => r.Product)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Date)
                .ToListAsync();

            return View(reviews);
        }

        // GET: Reviews/Add?productId=...
        public async Task<IActionResult> Add(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy thông tin sản phẩm
            var product = await _context.Products.FindAsync((long)productId);
            if (product == null)
            {
                return NotFound();
            }

            // Lấy tên khách hàng
            var user = await _context.Users.FindAsync(userId);
            var customerName = user?.UserName ?? "Khách hàng";

            ViewBag.ProductName = product.Name;

            var review = new ProductReview
            {
                ProductID = productId,
                UserId = userId,
                CustomerName = customerName
            };

            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductReview review)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User chưa đăng nhập, không thể đánh giá");
                return RedirectToAction("Login", "Account");
            }

            review.UserId = userId;

            // Kiểm tra xem khách hàng đã mua và nhận sản phẩm chưa
            var hasPurchased = await _context.Orders
                .Include(o => o.Lines)
                .AnyAsync(o =>
                    o.UserId == userId &&
                    o.Status == OrderStatus.DaNhanHang &&
                    o.Lines.Any(l => l.Product.ProductID == review.ProductID));

            if (!hasPurchased)
            {
                ModelState.AddModelError("", "Bạn chỉ có thể đánh giá sản phẩm đã mua và đã nhận hàng.");
            }

            // Kiểm tra xem đã đánh giá chưa
            var hasReviewed = await _context.ProductReviews
                .AnyAsync(r => r.UserId == userId && r.ProductID == review.ProductID);

            if (hasReviewed)
            {
                ModelState.AddModelError("", "Bạn đã đánh giá sản phẩm này rồi.");
            }

            if (!ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(review.ProductID);
                ViewBag.ProductName = product?.Name;
                return View(review);
            }

            review.Date = DateTime.Now;
            _context.ProductReviews.Add(review);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserId} đã đánh giá sản phẩm {ProductId}", userId, review.ProductID);

            return RedirectToAction("MyOrders", "Order");
        }

    }
}
