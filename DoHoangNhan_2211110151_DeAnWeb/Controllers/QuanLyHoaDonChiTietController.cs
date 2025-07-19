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
    public class QuanLyHoaDonChiTietController : Controller
    {
        WEBDB_DOHOANGNHAN_2211110151Entities db = new WEBDB_DOHOANGNHAN_2211110151Entities();

        // GET: QuanLyHoaDonChiTiet
        public ActionResult Index(string keyword, int page = 1, int pageSize = 6)
        {
            // Khởi tạo truy vấn ban đầu
            var query = db.TBL_HOADON_CHITIET.AsQueryable();

            // Kiểm tra từ khóa tìm kiếm và lọc dữ liệu
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(hdct => hdct.MAHD.Contains(keyword) ||
                                            hdct.MASP.Contains(keyword) ||
                                            hdct.TENSP.Contains(keyword));
            }

            // Thực hiện phân trang và chuyển đổi dữ liệu thành danh sách
            var model = query.OrderBy(hdct => hdct.IDCTHD).ToPagedList(page, pageSize);

            // Truyền từ khóa hiện tại vào ViewBag để hiển thị trong giao diện tìm kiếm
            ViewBag.CurrentKeyword = keyword;

            return View(model);
        }



        public ActionResult Create()
        {
            return View();
        }

        // POST: QuanLyHoaDonChiTiet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAHD, MASP, TENSP, GIATIEN, SOLUONG")] TBL_HOADON_CHITIET hoadonchitiet)
        {
            if (ModelState.IsValid)
            {
                hoadonchitiet.THANH_TIEN = hoadonchitiet.GIATIEN * hoadonchitiet.SOLUONG;
                db.TBL_HOADON_CHITIET.Add(hoadonchitiet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hoadonchitiet);
        }
        public ActionResult Edit(int ID)
        {
            TBL_HOADON_CHITIET _hoadonchitiet = db.TBL_HOADON_CHITIET.Find(ID);
            if (_hoadonchitiet == null)
            {
                return HttpNotFound();
            }
            return View(_hoadonchitiet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TBL_HOADON_CHITIET hoadonchitiet, int ID)
        {
            TBL_HOADON_CHITIET _hoadonchitiet_in_db = db.TBL_HOADON_CHITIET.Find(ID);
            if (_hoadonchitiet_in_db == null)
            {
                return HttpNotFound();
            }
            try
            {
                _hoadonchitiet_in_db.MAHD = hoadonchitiet.MAHD;
                _hoadonchitiet_in_db.MASP = hoadonchitiet.MASP;
                _hoadonchitiet_in_db.TENSP = hoadonchitiet.TENSP;
                _hoadonchitiet_in_db.THANG = hoadonchitiet.THANG;
                _hoadonchitiet_in_db.GIATIEN = hoadonchitiet.GIATIEN;
                _hoadonchitiet_in_db.SOLUONG = hoadonchitiet.SOLUONG;
                _hoadonchitiet_in_db.THANH_TIEN = hoadonchitiet.GIATIEN * hoadonchitiet.SOLUONG;

                UpdateModel(_hoadonchitiet_in_db);
                db.SaveChanges();
                ViewData["TRANGTHAICAPNHAT"] = "CẬP NHẬT THÀNH CÔNG";
                return View(_hoadonchitiet_in_db);
            }
            catch (Exception ex)
            {
                ViewData["TRANGTHAICAPNHAT"] = "LỖI: " + ex.Message.ToString();
                return View(hoadonchitiet);
            }
        }


        public ActionResult Details(int id)
        {
            TBL_HOADON_CHITIET hoadonchitiet = db.TBL_HOADON_CHITIET.Find(id);
            if (hoadonchitiet == null)
            {
                return HttpNotFound();
            }
            return View(hoadonchitiet);
        }

        public ActionResult Delete(int id)
        {
            TBL_HOADON_CHITIET hoadonchitiet = db.TBL_HOADON_CHITIET.Find(id);
            db.TBL_HOADON_CHITIET.Remove(hoadonchitiet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}