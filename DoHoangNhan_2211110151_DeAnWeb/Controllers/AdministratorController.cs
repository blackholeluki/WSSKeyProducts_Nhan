using DoHoangNhan_2211110151_DeAnWeb.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace DoHoangNhan_2211110151_DeAnWeb.Controllers
{
    public class AdministratorController : Controller
    {
        private const int MaxLoginAttempts = 5;
        private const int LockoutDurationSeconds = 30;

        // Action to display the login page
        public ActionResult Login()
        {
            TAIKHOAN tk = new TAIKHOAN();
            UpdateViewBag();

            return View(tk);
        }

        // POST action to handle login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TAIKHOAN tk)
        {
            if (ModelState.IsValid)
            {
                if (IsLockedOut())
                {
                    ViewBag.CountdownValue = Math.Ceiling(((DateTime)Session["LockoutEndTime"] - DateTime.Now).TotalSeconds);
                    ModelState.AddModelError("", $"Tài khoản bị khóa. Vui lòng đợi {ViewBag.CountdownValue} giây để thử lại.");
                }
                else if (IsValid(tk.EMAIL, tk.PASSWORD))
                {
                    ClearLockout(); // Clear lockout information on successful login
                    Session["Email"] = tk.EMAIL; // Save email to session
                    FormsAuthentication.SetAuthCookie(tk.EMAIL, false); // Create authentication cookie
                    return RedirectToAction("Index", "QuanLySanPham"); // Redirect to product management
                }
                else
                {
                    HandleFailedLogin(); // Handle the failed login attempt
                }
            }

            // Display the login page with error messages
            UpdateViewBag();
            return View(tk);
        }

        private void UpdateViewBag()
        {
            ViewBag.LoginAttemptsLeft = MaxLoginAttempts - GetLoginAttempts();
            ViewBag.ShowCountdownMessage = IsLockedOut();
            if (ViewBag.ShowCountdownMessage)
            {
                ViewBag.CountdownValue = Math.Ceiling(((DateTime)Session["LockoutEndTime"] - DateTime.Now).TotalSeconds);
            }
        }

        private void HandleFailedLogin()
        {
            IncrementLoginAttempts();
            int loginAttemptsLeft = MaxLoginAttempts - GetLoginAttempts();

            if (loginAttemptsLeft > 0)
            {
                ModelState.AddModelError("", $"Đăng nhập không thành công. Bạn còn {loginAttemptsLeft} lần thử.");
            }
            else
            {
                SetLockout(); // Lock the account for 30 seconds
                ViewBag.CountdownValue = LockoutDurationSeconds;
                ModelState.AddModelError("", $"Bạn đã nhập sai {MaxLoginAttempts} lần. Tài khoản bị khóa, vui lòng đợi {LockoutDurationSeconds} giây.");
            }
        }

        private bool IsValid(string email, string password)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            using (var db = new WEBDB_DOHOANGNHAN_2211110151Entities())
            {
                var user = db.TAIKHOANs.FirstOrDefault(u => u.EMAIL == email);
                if (user != null)
                {
                    return user.PASSWORD == crypto.Compute(password, user.PASSWORDSALT);
                }
            }
            return false; // Return false if user is null or password does not match
        }

        private bool IsLockedOut()
        {
            if (Session["LockoutEndTime"] != null)
            {
                var lockoutEndTime = (DateTime)Session["LockoutEndTime"];
                if (lockoutEndTime > DateTime.Now)
                {
                    return true; // Account is locked
                }
                ClearLockout(); // Clear lockout if the time has expired
            }
            return false;
        }

        private void IncrementLoginAttempts()
        {
            int attempts = (int?)Session["LoginAttempts"] ?? 0;
            attempts++;
            Session["LoginAttempts"] = attempts;
        }

        private void ClearLockout()
        {
            Session["LoginAttempts"] = 0; // Reset login attempts
            Session["LockoutEndTime"] = null; // Clear lockout time
        }

        private int GetLoginAttempts()
        {
            return (int?)Session["LoginAttempts"] ?? 0;
        }

        private void SetLockout()
        {
            Session["LockoutEndTime"] = DateTime.Now.AddSeconds(LockoutDurationSeconds); // Set lockout time for 30 seconds
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            return View("LogoutConfirmed");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogoutConfirmed()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Administrator");
        }
    }
}
