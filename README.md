# **"XÂY DỰNG WEBSITE BÁN CÁC SẢN PHẨM VỀ KEY PHẦN MỀM"**

---

### 1. Mục tiêu và công nghệ

* **Mục tiêu:** Tạo nền tảng bán và cho thuê key phần mềm hợp pháp, cung cấp cả lựa chọn mua dài hạn hoặc thuê ngắn hạn.
* **Công nghệ sử dụng:** ASP.NET MVC (C#), SQL Server, NuGet packages (PagedList, SimpleCrypto, PayPal SDK), hỗ trợ thanh toán PayPal/MoMo.
* **Kiến trúc:** Mô hình MVC (Model-View-Controller) giúp tách biệt giao diện, xử lý nghiệp vụ và dữ liệu.

---

### 2. Chức năng người dùng

* **Trang chủ:** Giới thiệu sản phẩm, đổi chế độ Light/Dark mode, menu điều hướng, danh sách sản phẩm bán chạy và sản phẩm mới.
* **Shop:** Danh mục sản phẩm, tìm kiếm, lọc theo giá/loại.
* **Chi tiết sản phẩm:** Thông tin, giá, tồn kho, thêm vào giỏ hàng, chọn số lượng.
* **Giỏ hàng:** Cập nhật số lượng, xóa sản phẩm, tổng giá trị.
* **Thanh toán:** Yêu cầu đăng nhập/đăng ký, nhập thông tin cá nhân, chọn phương thức (PayPal, MoMo), xác nhận đơn.
* **Hóa đơn:** Sau thanh toán hiển thị mã key, thông tin tài khoản, tổng tiền.
* **Lịch sử đơn hàng:** Xem lại các giao dịch trước, chi tiết sản phẩm và key.

---

### 3. Chức năng quản trị (Admin)

* **Đăng nhập quản trị:** Xác thực tài khoản, giới hạn thử sai (khóa 30 giây sau 5 lần sai).
* **Quản lý sản phẩm:** Thêm, sửa, xóa, xem chi tiết, quản lý tồn kho, hình ảnh, key.
* **Quản lý hóa đơn:** Danh sách giao dịch, chỉnh sửa, thêm, xóa.
* **Quản lý chi tiết hóa đơn:** Cập nhật từng sản phẩm trong hóa đơn (mã sản phẩm, số lượng, thành tiền).
* **Quản lý tài khoản:** Thêm tài khoản, chỉnh sửa quyền truy cập, xóa, xem chi tiết.
* **Bảo mật:** Mã hóa mật khẩu (PBKDF2 – SimpleCrypto), phân quyền (user, admin).

---

### 4. Cơ sở dữ liệu

* **Các bảng chính:**

  * `SANPHAM`, `LOAISANPHAM`, `HOADON`, `HOADONCHITIET`, `TAIKHOAN`, `MAKEY`.
  * Lưu trữ thông tin sản phẩm, hóa đơn, key, tài khoản, tình trạng sử dụng key.
* **Chức năng quan hệ:** Ràng buộc khóa chính/khóa ngoại, liên kết bảng sản phẩm – loại – hóa đơn – key.

---

### 5. Giao diện

* Có hình ảnh minh họa: trang chủ, shop, popup giỏ hàng, trang thanh toán, giao diện admin, phân trang, tìm kiếm, thêm sản phẩm, xem hóa đơn.

