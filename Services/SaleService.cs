using SportsStore.Models;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Services
{
    public class SaleService
    {
        private readonly StoreDbContext _context;

        public SaleService(StoreDbContext context)
        {
            _context = context;
        }

        // Lấy 5 sản phẩm đang sale (tự động đổi mỗi giờ)
        public List<Product> GetCurrentSaleProducts()
        {
            var now = DateTime.Now;
            var currentHour = now.Hour;
            var allProducts = _context.Products
                .Where(p => p.IsForSale && p.Quantity > 0)
                .OrderBy(p => p.ProductID)
                .ToList();

            if (!allProducts.Any())
                return new List<Product>();

            // Chọn 5 sản phẩm dựa trên giờ hiện tại
            var startIndex = (currentHour * 5) % allProducts.Count;
            var saleProducts = new List<Product>();
            
            // Tính thời gian sale cho giờ hiện tại
            var startTime = new DateTime(now.Year, now.Month, now.Day, currentHour, 0, 0);
            var endTime = startTime.AddHours(1); // Kết thúc vào đầu giờ tiếp theo (ví dụ 14h00)
            
            for (int i = 0; i < 5 && i < allProducts.Count; i++)
            {
                var index = (startIndex + i) % allProducts.Count;
                var product = allProducts[index];
                
                // Kiểm tra xem sale hiện tại có thuộc giờ hiện tại không
                bool needsUpdate = false;
                
                if (!product.SalePrice.HasValue || 
                    !product.SaleStartTime.HasValue || 
                    product.SaleEndTime < now)
                {
                    // Chưa có sale hoặc đã hết hạn
                    needsUpdate = true;
                }
                else if (product.SaleStartTime.Value.Hour != currentHour)
                {
                    // Sale không thuộc giờ hiện tại (ví dụ sale 13h nhưng giờ là 14h)
                    needsUpdate = true;
                }
                
                if (needsUpdate)
                {
                    var salePercent = 20; // Giảm 20%
                    product.SalePrice = product.Price * (100 - salePercent) / 100;
                    product.SaleStartTime = startTime;
                    product.SaleEndTime = endTime;
                }
                
                saleProducts.Add(product);
            }
            
            _context.SaveChanges();
            return saleProducts;
        }

        // Xem trước sản phẩm sale khung giờ tiếp theo
        public List<Product> GetNextHourSaleProducts()
        {
            var nextHour = DateTime.Now.Hour + 1;
            if (nextHour >= 24) nextHour = 0;
            
            var allProducts = _context.Products
                .Where(p => p.IsForSale && p.Quantity > 0)
                .OrderBy(p => p.ProductID)
                .ToList();

            if (!allProducts.Any())
                return new List<Product>();

            var startIndex = (nextHour * 5) % allProducts.Count;
            var nextSaleProducts = new List<Product>();
            
            for (int i = 0; i < 5 && i < allProducts.Count; i++)
            {
                var index = (startIndex + i) % allProducts.Count;
                var product = allProducts[index];
                
                // Tính giá sale cho khung giờ tiếp theo (không lưu vào DB)
                var salePercent = 20;
                var previewSalePrice = product.Price * (100 - salePercent) / 100;
                
                // Tạo bản sao để preview (không ảnh hưởng DB)
                var previewProduct = new Product
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    Price = product.Price,
                    SalePrice = previewSalePrice,
                    Image = product.Image,
                    Category = product.Category,
                    Quantity = product.Quantity
                };
                
                nextSaleProducts.Add(previewProduct);
            }
            
            return nextSaleProducts;
        }

        // Reset sale cho tất cả sản phẩm (gọi mỗi giờ)
        public void ResetSales()
        {
            var now = DateTime.Now;
            var currentHour = now.Hour;
            
            // Reset tất cả sale đã hết hạn hoặc không thuộc giờ hiện tại
            var expiredSales = _context.Products
                .Where(p => p.SaleEndTime.HasValue && 
                           (p.SaleEndTime < now || 
                            (p.SaleStartTime.HasValue && p.SaleStartTime.Value.Hour != currentHour)))
                .ToList();

            foreach (var product in expiredSales)
            {
                product.SalePrice = null;
                product.SaleStartTime = null;
                product.SaleEndTime = null;
            }

            _context.SaveChanges();
        }
    }
}

