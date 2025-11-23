# Hướng Dẫn Test Persistent Cart

## Tính năng đã implement:
✅ Lưu giỏ hàng vào database theo UserId
✅ Giỏ hàng persistent qua các session (đăng xuất/đăng nhập vẫn giữ)
✅ Merge giỏ hàng từ session với database khi đăng nhập
✅ Anonymous users vẫn dùng session cart bình thường

## Cách test:

### Test 1: Cart được lưu khi đăng nhập
1. Đăng nhập vào tài khoản (ví dụ: admin/Secret123$)
2. Thêm 2-3 sản phẩm vào giỏ hàng
3. Kiểm tra database: `SELECT * FROM UserCartItems` → Sẽ thấy các item với UserId

### Test 2: Cart persistent qua session
1. Đăng nhập và thêm sản phẩm vào giỏ
2. Đăng xuất
3. Đóng trình duyệt (clear session)
4. Mở lại và đăng nhập cùng tài khoản
5. ✅ Giỏ hàng vẫn còn đầy đủ sản phẩm

### Test 3: Merge cart khi đăng nhập
1. Chưa đăng nhập, thêm 1-2 sản phẩm vào giỏ (lưu trong session)
2. Đăng nhập vào tài khoản đã có sẵn cart trong database
3. ✅ Giỏ hàng sẽ merge cả 2 (session + database)
   - Nếu trùng sản phẩm → cộng số lượng
   - Nếu khác sản phẩm → thêm vào

### Test 4: Anonymous user vẫn hoạt động
1. Không đăng nhập
2. Thêm sản phẩm vào giỏ
3. ✅ Cart vẫn hoạt động bình thường (lưu trong session)
4. Reload trang → Cart vẫn còn (trong cùng session)

### Test 5: Clear cart
1. Đăng nhập và có items trong giỏ
2. Checkout xong hoặc xóa hết giỏ hàng
3. ✅ Database cũng xóa các items tương ứng

## Database Schema:

```sql
CREATE TABLE UserCartItems (
    UserCartItemId INT PRIMARY KEY IDENTITY,
    UserId NVARCHAR(450) NOT NULL,
    ProductId BIGINT NOT NULL,
    Quantity INT NOT NULL,
    IsRental BIT NOT NULL DEFAULT 0,
    RentalDays INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductID)
)
```

## Console logs để debug:

Khi thêm/xóa sản phẩm, console sẽ hiển thị:
- `[PersistentCart] Loaded X items from database for user {userId}`
- `[PersistentCart] Saved X items to database for user {userId}`
- `[PersistentCart] Migrated session cart to database for user {userId}`

## Lưu ý:
- User phải đăng nhập để cart lưu vào database
- Anonymous users cart chỉ tồn tại trong session
- Khi đăng nhập, session cart tự động merge vào database cart
- Clear browser data sẽ mất session cart nhưng database cart vẫn giữ nguyên
