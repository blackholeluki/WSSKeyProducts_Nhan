# 🔑 Xây dựng Website Bán và Cho Thuê Sản Phẩm về Key Phần Mềm

## 📌 Giới thiệu

Dự án phát triển một nền tảng thương mại điện tử chuyên cung cấp **key phần mềm bản quyền** và các dịch vụ thuê tài khoản (Netflix, Spotify, Windows License...). Hệ thống hỗ trợ **bán trực tiếp và cho thuê** theo nhu cầu, giúp người dùng **tiết kiệm chi phí**, tiếp cận phần mềm **hợp pháp và an toàn**.

---

## 🎯 Mục tiêu

* Cung cấp **giải pháp mua/thuê key phần mềm** tiện lợi, hợp pháp, nhanh chóng.
* Xây dựng website **ASP.NET MVC** với **cơ sở dữ liệu SQL Server**.
* Hỗ trợ **thanh toán trực tuyến** (PayPal, MoMo) và **quản trị sản phẩm, hóa đơn, tài khoản**.
* Tăng cường **bảo mật thông tin** (mã hóa mật khẩu PBKDF2 – SimpleCrypto).
* Thiết kế giao diện **responsive, dark/light mode**, dễ sử dụng.

---

## 🛠️ Công nghệ sử dụng

* **Ngôn ngữ & Framework:** C#, ASP.NET MVC 5
* **CSDL:** Microsoft SQL Server
* **Công cụ:** Visual Studio 2022, SSMS
* **Thư viện/NuGet Packages:**

  * `PagedList.Mvc`: phân trang danh sách sản phẩm
  * `SimpleCrypto`: mã hóa mật khẩu
  * `PayPal SDK`: tích hợp thanh toán
* **Kiến trúc:** Mô hình MVC (Model – View – Controller)

---

## ⚙️ Chức năng chính

### 1. Người dùng (Khách hàng)

* Đăng ký/đăng nhập tài khoản.
* Duyệt sản phẩm, tìm kiếm, lọc theo loại/giá.
* Quản lý **giỏ hàng**: thêm, sửa số lượng, xóa.
* **Thanh toán trực tuyến**: chọn PayPal hoặc MoMo.
* Nhận **hóa đơn điện tử** kèm key/tài khoản sau khi thanh toán.
* Xem **lịch sử đơn hàng** và tải lại key.

### 2. Quản trị viên (Admin)

* **Quản lý sản phẩm:** thêm, sửa, xóa, quản lý tồn kho, gán key.
* **Quản lý hóa đơn:** theo dõi, chỉnh sửa, xuất file, xóa khi cần.
* **Quản lý chi tiết hóa đơn:** kiểm tra sản phẩm, số lượng, giá.
* **Quản lý tài khoản:** tạo user, phân quyền (admin/user), reset mật khẩu.
* **Bảo mật:** khóa tài khoản 30 giây nếu nhập sai >5 lần.

---

## 🗃️ Thiết kế cơ sở dữ liệu

**Các bảng chính:**

* `SANPHAM (IDSP, MASP, TENSP, GIATIEN, SO_LUONG_TON, THANG…)`
* `LOAISANPHAM (IDLOAI, TENLOAI, HINHANH)`
* `HOADON (IDHD, MAHD, TENSP, SOLUONG, THANHTIEN…)`
* `HOADONCHITIET (IDCTHD, MAHD, MASP, SỐ_LƯỢNG, GIÁ, THÀNH_TIỀN)`
* `TAIKHOAN (EMAIL, PASSWORD, ACCESS…)`
* `MAKEY (ID, MASP, MAKEY, IS_USED)`

---

## 🖥️ Giao diện chính

**Front-end:**

* Trang chủ: banner, sản phẩm nổi bật, dark/light mode.
* Trang shop: danh sách key, công cụ lọc, phân trang.
* Giỏ hàng, thanh toán, popup thông báo.
* Trang hóa đơn & lịch sử mua hàng.

**Admin Panel:**

* Đăng nhập bảo mật.
* Quản lý sản phẩm, hóa đơn, chi tiết hóa đơn, tài khoản.
* Thống kê nhanh (sản phẩm tồn kho, giao dịch).

---

## 🚀 Hướng dẫn cài đặt & chạy website

### 1. Yêu cầu hệ thống

* Windows 10/11
* Visual Studio 2019/2022
* SQL Server 2019
* .NET Framework ≥ 4.7.2

### 2. Cài đặt

1. Clone repo về máy:

   ```bash
   git clone https://github.com/<your-repo>/KeySoftwareShop.git
   ```
2. Import database:

   * Mở `SQL Server Management Studio`.
   * Tạo DB `KeySoftwareDB` và chạy script `.sql` trong thư mục `/Database`.
3. Mở file `Web.config` → sửa chuỗi kết nối:

   ```xml
   <connectionStrings>
       <add name="KeySoftwareDB"
            connectionString="Data Source=YOUR_SERVER;Initial Catalog=KeySoftwareDB;Integrated Security=True"/>
   </connectionStrings>
   ```
4. Build & chạy:

   ```bash
   F5
   ```
5. Truy cập: `http://localhost:xxxx`

---

## 🔑 Tài khoản mẫu

* **Admin:** email: `user@gmail.com`, pass: `123456`
* **User:** đăng ký trong website để có tài khoản user

