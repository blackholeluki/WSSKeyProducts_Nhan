# 🔑 XÂY DỰNG WEBSITE BÁN SẢN PHẨM VỀ KEY PHẦN MỀM

## 📖 1. Giới thiệu

Dự án này xây dựng một **website thương mại điện tử** chuyên cung cấp **key phần mềm bản quyền** và dịch vụ **cho thuê tài khoản phần mềm**.
Hệ thống được thiết kế để:

* **Hỗ trợ người dùng cá nhân và doanh nghiệp** dễ dàng tiếp cận phần mềm hợp pháp với chi phí tối ưu.
* Cung cấp **tính năng mua & thuê key phần mềm**, **thanh toán trực tuyến**, **quản lý hóa đơn – tài khoản – sản phẩm**, tất cả trong một nền tảng duy nhất.
* Đảm bảo **bảo mật dữ liệu**, **quản lý kho key tự động**, và **giao diện thân thiện**.

---

## 🎯 2. Mục tiêu dự án

* Xây dựng website bán và cho thuê key phần mềm **đầy đủ chức năng thương mại điện tử**.
* **Tích hợp thanh toán trực tuyến**: PayPal, MoMo.
* **Quản lý sản phẩm, hóa đơn, kho key, tài khoản người dùng** một cách hiệu quả.
* Thiết kế **UI/UX hiện đại**, hỗ trợ **Dark/Light Mode**, responsive.
* Đảm bảo **an toàn, bảo mật và phân quyền** cho hệ thống.

---

## 🛠️ 3. Công nghệ & công cụ

| Thành phần         | Công nghệ sử dụng                             |
| ------------------ | --------------------------------------------- |
| Ngôn ngữ           | **C#, ASP.NET MVC 5**                         |
| Cơ sở dữ liệu      | **Microsoft SQL Server 2019**                 |
| IDE                | **Visual Studio 2022**                        |
| ORM                | LINQ to SQL                                   |
| Thư viện / Package | `PagedList.Mvc`, `SimpleCrypto`, `PayPal SDK` |
| Kiến trúc          | **MVC (Model – View – Controller)**           |
| Triển khai         | IIS / Azure / Localhost                       |

---

## ⚙️ 4. Chức năng chi tiết

### 4.1. Chức năng người dùng (Front-end)

* **Đăng ký/Đăng nhập tài khoản** (email, mật khẩu mã hóa PBKDF2).
* **Duyệt sản phẩm**: tìm kiếm, lọc theo loại, giá, sắp xếp.
* **Giỏ hàng**:

  * Thêm/xóa/sửa số lượng sản phẩm.
  * Hiển thị tổng tiền theo thời gian thực.
* **Thanh toán trực tuyến**:

  * PayPal, MoMo.
  * Kiểm tra đăng nhập trước khi thanh toán.
  * Xác nhận giao dịch, xuất **hóa đơn điện tử**.
* **Nhận key/tài khoản sau thanh toán**:

  * Hệ thống tự động cấp phát key chưa sử dụng (`IS_USED = 0`) và cập nhật trạng thái (`IS_USED = 1`).
* **Lịch sử mua hàng**: xem lại hóa đơn, tải lại key, quản lý thông tin giao dịch.

---

### 4.2. Chức năng quản trị (Admin Panel)

* **Quản lý sản phẩm**:

  * CRUD (Create – Read – Update – Delete).
  * Cập nhật giá, tồn kho, hình ảnh.
* **Quản lý loại sản phẩm**: thêm/sửa/xóa danh mục.
* **Quản lý hóa đơn**:

  * Xem danh sách, chỉnh sửa, xóa.
  * Thêm thủ công hóa đơn (giao dịch ngoài website).
* **Quản lý chi tiết hóa đơn**:

  * Xem các sản phẩm trong từng hóa đơn.
  * Chỉnh sửa số lượng, thành tiền.
* **Quản lý tài khoản**:

  * Tạo mới user/admin.
  * Reset mật khẩu.
  * Phân quyền (`ACCESS: 0=user, 1=admin`).
* **Bảo mật**:

  * Khóa tài khoản 30 giây nếu nhập sai mật khẩu >5 lần.
  * Mã hóa mật khẩu bằng `SimpleCrypto` (PBKDF2).

---

## 🗃️ 5. Thiết kế cơ sở dữ liệu

### 5.1. Các bảng chính

* **SANPHAM**: lưu sản phẩm (IDSP, MASP, TENSP, GIATIEN, SO\_LUONG\_TON, THANG).
* **LOAISANPHAM**: quản lý danh mục.
* **HOADON**: lưu thông tin giao dịch.
* **HOADONCHITIET**: chi tiết từng sản phẩm trong hóa đơn.
* **TAIKHOAN**: thông tin user/admin (EMAIL, PASSWORD, ACCESS).
* **MAKEY**: quản lý key phần mềm (MASP, MAKEY, IS\_USED).

### 5.2. Quan hệ

* `LOAISANPHAM` – 1\:N – `SANPHAM`
* `SANPHAM` – 1\:N – `HOADONCHITIET`
* `HOADON` – 1\:N – `HOADONCHITIET`
* `SANPHAM` – 1\:N – `MAKEY`
* `TAIKHOAN` – 1\:N – `HOADON`

---

## 🖥️ 6. Giao diện

### 6.1. Người dùng

* Trang chủ: banner, sản phẩm hot, chế độ Dark/Light.
* Shop: danh sách sản phẩm, phân trang, tìm kiếm.
* Giỏ hàng: cập nhật số lượng, tổng tiền.
* Thanh toán: chọn phương thức, xác nhận, xuất hóa đơn.
* Lịch sử đơn hàng: xem & tải lại key.

### 6.2. Admin

* Dashboard: thống kê sản phẩm – đơn hàng.
* Quản lý sản phẩm, hóa đơn, key, tài khoản.
* Tìm kiếm, lọc dữ liệu nhanh.

---

## 🚀 7. Hướng dẫn cài đặt & triển khai

### 7.1. Yêu cầu hệ thống

* Windows 10/11.
* Visual Studio 2019/2022.
* SQL Server 2019.
* .NET Framework ≥ 4.7.2.

### 7.2. Các bước cài đặt

1. **Clone dự án:**

   ```bash
   git clone https://github.com/<your-username>/KeySoftwareShop.git
   ```
2. **Khởi tạo CSDL:**

   * Mở `SSMS`.
   * Tạo database `KeySoftwareDB`.
   * Chạy file `Database/KeySoftwareDB.sql`.
3. **Cấu hình connection string:**

   ```xml
   <connectionStrings>
       <add name="KeySoftwareDB"
            connectionString="Data Source=YOUR_SERVER;Initial Catalog=KeySoftwareDB;Integrated Security=True"/>
   </connectionStrings>
   ```
4. **Chạy website:**

   ```bash
   # Visual Studio:
   Build > Rebuild Solution
   Run (F5)
   ```
5. Truy cập: `http://localhost:xxxx`

---

## 🔑 8. Tài khoản mẫu

* **Admin:**

  * Email: `admin@hbmarket.com`
  * Password: `admin123`
* **User:**

  * Email: `user@hbmarket.com`
  * Password: `123456`

---

## 📂 9. Cấu trúc thư mục dự án

```
KeySoftwareShop/
│── Controllers/
│── Models/
│── Views/
│── Scripts/
│── Content/
│── Database/
│   └── KeySoftwareDB.sql
│── App_Start/
│── Web.config
│── README.md
```

---

## 🌐 10. Hướng dẫn triển khai hosting

### 10.1. IIS

* Cài **IIS + .NET Framework Hosting Bundle**.
* Publish project → Deploy vào IIS → Gán domain nội bộ.

### 10.2. Azure (Cloud)

* Publish trực tiếp từ Visual Studio.
* Kết nối với Azure SQL Database.
* Cập nhật connection string trong `Web.config`.
