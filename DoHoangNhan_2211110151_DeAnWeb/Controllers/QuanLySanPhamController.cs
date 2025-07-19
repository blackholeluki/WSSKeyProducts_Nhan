using DoHoangNhan_2211110151_DeAnWeb.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoHoangNhan_2211110151_DeAnWeb.Controllers
{

    [Authorize]
    public class QuanLySanPhamController : Controller
    {
        WEBDB_DOHOANGNHAN_2211110151Entities db = new WEBDB_DOHOANGNHAN_2211110151Entities();

        // GET: QuanLySanPham
        public ActionResult Index(string keyword, int page = 1, int pageSize = 10)
        {
            IQueryable<TBL_SANPHAM> products = db.TBL_SANPHAM;

            // Nếu có từ khóa tìm kiếm, lọc danh sách sản phẩm
            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.TENSP.Contains(keyword));
                ViewBag.CurrentKeyword = keyword; // Lưu từ khóa hiện tại để hiển thị lại trong view
            }

            var model = products.OrderBy(p => p.IDSP).ToPagedList(page, pageSize);
            return View(model);
        }

        public JsonResult GetProductSuggestions(string keyword)
        {
            var suggestions = db.TBL_SANPHAM
                                .Where(p => p.TENSP.Contains(keyword))
                                .Select(p => new { p.IDSP, p.TENSP }) // Chỉ trả về ID và tên sản phẩm
                                .Take(5) // Giới hạn số lượng gợi ý (ở đây là 5)
                                .ToList();

            return Json(suggestions, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create()
        {
            TBL_SANPHAM _sp = new TBL_SANPHAM();

            // Lấy danh sách loại sản phẩm và truyền vào ViewBag
            ViewBag.LoaiSanPhamList = new SelectList(db.TBL_LOAISANPHAM, "IDLOAI", "TENLOAI");

            return View(_sp);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(TBL_SANPHAM sanpham, string[] keys)
        {
            if (ModelState.IsValid)
            {
                db.TBL_SANPHAM.Add(sanpham);
                db.SaveChanges();

                foreach (var key in keys) // Add new keys
                {
                    db.TBL_MAKEY.Add(new TBL_MAKEY { MASP = sanpham.MASP, MAKEY = key, IS_USED = false });
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Load lại danh sách loại sản phẩm nếu có lỗi
            ViewBag.LoaiSanPhamList = new SelectList(db.TBL_LOAISANPHAM, "IDLOAI", "TENLOAI", sanpham.IDLOAI);
            return View(sanpham);
        }


        public ActionResult Edit(int ID)
        {
            TBL_SANPHAM sp = db.TBL_SANPHAM.Find(ID);
            if (sp == null)
                return HttpNotFound();

            // Load existing keys to display
            ViewBag.ExistingKeys = db.TBL_MAKEY.Where(k => k.MASP == sp.MASP && k.IS_USED == false).Select(k => k.MAKEY).ToList();

            return View(sp);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(TBL_SANPHAM sanpham, string[] keys)
        {
            TBL_SANPHAM sp_in_db = db.TBL_SANPHAM.Find(sanpham.IDSP);
            if (sp_in_db == null)
                return HttpNotFound();

            try
            {
                // Cập nhật thông tin sản phẩm
                UpdateModel(sp_in_db);

                if (keys != null && keys.Any())
                {
                    // Xử lý các mã key mới nếu có
                    var existingKeys = db.TBL_MAKEY.Where(k => k.MASP == sanpham.MASP).ToList();
                    db.TBL_MAKEY.RemoveRange(existingKeys); // Xóa mã key cũ
                    foreach (var key in keys) // Thêm mã key mới
                    {
                        db.TBL_MAKEY.Add(new TBL_MAKEY { MASP = sanpham.MASP, MAKEY = key, IS_USED = false });
                    }
                }

                db.SaveChanges();
                ViewData["TRANGTHAICAPNHAT"] = "CẬP NHẬT THÀNH CÔNG";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["TRANGTHAICAPNHAT"] = "Error: " + ex.Message;
                return View(sanpham);
            }
        }


        public ActionResult Details(int id)
        {
            TBL_SANPHAM product = db.TBL_SANPHAM.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            // Fetch keys associated with the product
            var productKeys = db.TBL_MAKEY.Where(k => k.MASP == product.MASP && k.IS_USED == false).ToList();
            ViewBag.ProductKeys = productKeys; // Passing keys to the view through ViewBag

            return View(product);
        }


        public ActionResult Delete(int id)
        {
            TBL_SANPHAM product = db.TBL_SANPHAM.Find(id);
            db.TBL_SANPHAM.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}