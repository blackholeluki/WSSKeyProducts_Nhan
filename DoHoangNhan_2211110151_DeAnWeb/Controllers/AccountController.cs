using DoHoangNhan_2211110151_DeAnWeb.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

public class AccountController : Controller
{
    private readonly WEBDB_DOHOANGNHAN_2211110151Entities db = new WEBDB_DOHOANGNHAN_2211110151Entities();

    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Register(string email, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            ViewBag.Message = "Passwords do not match.";
            return View();
        }

        var existingUser = db.TAIKHOANs.SingleOrDefault(u => u.EMAIL == email);
        if (existingUser != null)
        {
            ViewBag.Message = "Email already exists.";
            return View();
        }

        var passwordSalt = GenerateSalt();
        var hashedPassword = HashPassword(password, passwordSalt);

        var user = new TAIKHOAN
        {
            EMAIL = email,
            PASSWORD = hashedPassword,
            PASSWORDSALT = passwordSalt,
            ACCESS = 0
        };

        db.TAIKHOANs.Add(user);
        db.SaveChanges();
        Session["UserEmail"] = user.EMAIL;
        return RedirectToAction("Index", "Home");
    }


    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public JsonResult Login(string email, string password)
    {
        var user = db.TAIKHOANs.SingleOrDefault(u => u.EMAIL == email);
        if (user == null || !VerifyPassword(password, user.PASSWORD, user.PASSWORDSALT))
        {
            return Json(new { success = false, message = "Tài khoản hoặc mật khẩu sai." });
        }

        Session["UserEmail"] = user.EMAIL;
        return Json(new { success = true });
    }

    public ActionResult Logout()
    {
        Session["UserEmail"] = null;
        return RedirectToAction("Index", "Home");
    }

    private bool VerifyPassword(string password, string storedHash, string salt)
    {
        string hashedPassword = HashPassword(password, salt);
        return hashedPassword == storedHash;
    }

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


    public ActionResult UserOrders()
    {
        if (Session["UserEmail"] == null)
        {
            return RedirectToAction("Login", "Account");
        }
        string userEmail = Session["UserEmail"].ToString();
        var userOrders = db.TBL_HOADON
                            .Where(h => h.EMAIL == userEmail && h.TRANG_THAI == 1)
                            .OrderByDescending(h => h.NGAYBAN) // Sắp xếp từ mới đến cũ
                            .ToList();
        return View(userOrders);
    }

}
