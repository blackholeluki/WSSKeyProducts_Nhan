using DoHoangNhan_2211110151_DeAnWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoHoangNhan_2211110151_DeAnWeb.Controllers
{
    public class SanPhamController : Controller
    {
        WEBDB_DOHOANGNHAN_2211110151Entities db = new WEBDB_DOHOANGNHAN_2211110151Entities();
        // GET: SanPham
        public ActionResult DanhSachSanPhamTheoLoai(int idLoai)
        {
            var categories = db.TBL_LOAISANPHAM.ToList();
            ViewBag.Categories = categories;
            using (var db = new WEBDB_DOHOANGNHAN_2211110151Entities())
            {
                var danhSachSanPham = db.TBL_SANPHAM.Where(sp => sp.IDLOAI == idLoai).ToList();
                var loaiSanPham = db.TBL_LOAISANPHAM.Find(idLoai);

                if (loaiSanPham != null)
                {
                    ViewBag.TenLoaiSanPham = loaiSanPham.TENLOAI; // Truyền tên loại sản phẩm vào ViewBag
                }

                return View(danhSachSanPham);
            }
        }




    }
}