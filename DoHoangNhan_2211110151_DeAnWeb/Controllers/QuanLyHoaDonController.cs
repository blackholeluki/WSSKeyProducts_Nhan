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
    public class QuanLyHoaDonController : Controller
    {
        WEBDB_DOHOANGNHAN_2211110151Entities db = new WEBDB_DOHOANGNHAN_2211110151Entities();

        // GET: QuanLyHoaDon
        public ActionResult Index(int page = 1, int pageSize = 6, string keyword = "")
        {
            var query = db.TBL_HOADON.AsQueryable();

            // Lọc theo các tiêu chí tìm kiếm chỉ bao gồm mã hóa đơn, tên khách hàng, số điện thoại và email
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(hd => hd.MAHD.Contains(keyword) ||
                                          hd.TEN_KHACH_HANG.Contains(keyword) ||
                                          hd.SDT.Contains(keyword) ||
                                          hd.EMAIL.Contains(keyword));
            }

            // Chuyển đổi truy vấn thành danh sách và phân trang
            var lhoadon = query.OrderBy(hd => hd.IDHD).ToPagedList(page, pageSize);

            return View(lhoadon);
        }






        public ActionResult Create()
        {
            TBL_HOADON _hoadon = new TBL_HOADON();
            return View(_hoadon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TBL_HOADON hoadon)
        {
            if (ModelState.IsValid)
            {
                db.TBL_HOADON.Add(hoadon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hoadon);
        }

        public ActionResult Edit(int ID)
        {
            TBL_HOADON _hoadon = db.TBL_HOADON.Find(ID);
            if (_hoadon == null)
            {
                return HttpNotFound();
            }
            return View(_hoadon);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TBL_HOADON hoadon)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var _hoadon_in_db = db.TBL_HOADON.Find(hoadon.IDHD);
                    if (_hoadon_in_db == null)
                    {
                        return HttpNotFound();
                    }

                    _hoadon_in_db.MAHD = hoadon.MAHD;
                    _hoadon_in_db.TEN_KHACH_HANG = hoadon.TEN_KHACH_HANG;
                    _hoadon_in_db.EMAIL = hoadon.EMAIL;
                    _hoadon_in_db.SDT = hoadon.SDT;
                    _hoadon_in_db.TONG_THANH_TIEN = hoadon.TONG_THANH_TIEN;
                    _hoadon_in_db.TRANG_THAI = hoadon.TRANG_THAI;
                    _hoadon_in_db.NGAYBAN = hoadon.NGAYBAN;

                    db.SaveChanges();

                    // Set ViewData for success message
                    ViewData["TRANGTHAICAPNHAT"] = "Cập nhật thành công";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewData["TRANGTHAICAPNHAT"] = "Cập nhật thất bại: " + ex.Message;
                    return View(hoadon);
                }
            }

            return View(hoadon);
        }

        public ActionResult Details(int id)
        {
            TBL_HOADON hoadon = db.TBL_HOADON.Find(id);
            if (hoadon == null)
            {
                return HttpNotFound();
            }
            return View(hoadon);
        }

        public ActionResult Delete(int id)
        {
            TBL_HOADON hoadon = db.TBL_HOADON.Find(id);
            db.TBL_HOADON.Remove(hoadon);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}