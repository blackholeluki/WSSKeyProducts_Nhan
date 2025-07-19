using DoHoangNhan_2211110151_DeAnWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace DoHoangNhan_2211110151_DeAnWeb.Controllers
{
    [Authorize]
    public class QuanLyAccountController : Controller
    {
        private readonly WEBDB_DOHOANGNHAN_2211110151Entities db = new WEBDB_DOHOANGNHAN_2211110151Entities();

        public ActionResult Index(int page = 1, int pageSize = 6, string keyword = "")
        {
            var query = db.TAIKHOANs.AsQueryable();

            // Tìm kiếm theo email hoặc quyền truy cập
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(acc => acc.EMAIL.Contains(keyword) ||
                                           acc.ACCESS.ToString().Contains(keyword));
            }

            // Phân trang danh sách tài khoản
            var accounts = query.OrderBy(acc => acc.EMAIL).ToPagedList(page, pageSize);

            return View(accounts);
        }
        // Tạo mới tài khoản
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string email, string password, string confirmPassword, int access)
        {
            if (password != confirmPassword)
            {
                ViewBag.Message = "Mật khẩu và xác nhận mật khẩu không khớp.";
                return View();
            }

            if (db.TAIKHOANs.Any(u => u.EMAIL == email))
            {
                ViewBag.Message = "Email đã tồn tại.";
                return View();
            }

            var passwordSalt = GenerateSalt();
            var hashedPassword = HashPassword(password, passwordSalt);

            var newAccount = new TAIKHOAN
            {
                EMAIL = email,
                PASSWORD = hashedPassword,
                PASSWORDSALT = passwordSalt,
                ACCESS = access
            };

            db.TAIKHOANs.Add(newAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Chỉnh sửa tài khoản
        [HttpGet]
        public ActionResult Edit(string id)
        {
            // Giải mã id từ Base64 thành email
            var email = Encoding.UTF8.GetString(Convert.FromBase64String(id));
            var account = db.TAIKHOANs.SingleOrDefault(a => a.EMAIL == email);

            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        [HttpPost]
        public ActionResult Edit(string email, string password, string confirmPassword, int access)
        {
            var account = db.TAIKHOANs.SingleOrDefault(a => a.EMAIL == email);
            if (account == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra mật khẩu xác nhận
            if (!string.IsNullOrEmpty(password) && password != confirmPassword)
            {
                ViewBag.Message = "Mật khẩu và xác nhận mật khẩu không khớp.";
                return View(account);
            }

            if (!string.IsNullOrEmpty(password))
            {
                var passwordSalt = GenerateSalt();
                account.PASSWORDSALT = passwordSalt;
                account.PASSWORD = HashPassword(password, passwordSalt);
            }

            account.ACCESS = access;

            db.SaveChanges();
            ViewData["TRANGTHAICAPNHAT"] = "Cập nhật tài khoản thành công!";
            return View(account);
        }


        // Xóa tài khoản (không được xóa chính mình)
        public ActionResult Delete(string id)
        {
            var email = Encoding.UTF8.GetString(Convert.FromBase64String(id));
            var account = db.TAIKHOANs.Find(email);
            if (account == null)
            {
                return HttpNotFound();
            }
            db.TAIKHOANs.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }





        public ActionResult Details(string id)
        {
            // Giải mã id từ Base64 thành email
            var email = Encoding.UTF8.GetString(Convert.FromBase64String(id));
            var account = db.TAIKHOANs.SingleOrDefault(a => a.EMAIL == email);

            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }





        // Các hàm hỗ trợ bảo mật mật khẩu
        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] combinedBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}