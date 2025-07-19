using DoHoangNhan_2211110151_DeAnWeb.Models;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace DoHoangNhan_2211110151_DeAnWeb.Controllers
{
    [Authorize]
    public class QuanLyLoaiSanPhamController : Controller
    {
        private readonly WEBDB_DOHOANGNHAN_2211110151Entities db = new WEBDB_DOHOANGNHAN_2211110151Entities();

        // GET: QuanLyLoaiSanPham
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var loaiSanPhams = db.TBL_LOAISANPHAM.OrderBy(x => x.IDLOAI).ToPagedList(page, pageSize);
            return View(loaiSanPhams);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TBL_LOAISANPHAM loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                db.TBL_LOAISANPHAM.Add(loaiSanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(loaiSanPham);
        }

        // GET: QuanLyLoaiSanPham/Edit/8
        public ActionResult Edit(int id)
        {
            var loaiSanPham = db.TBL_LOAISANPHAM.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound(); // Nếu không tìm thấy, trả về lỗi 404
            }
            return View(loaiSanPham);
        }

        // POST: QuanLyLoaiSanPham/Edit/8
        [HttpPost]
        public ActionResult Edit(TBL_LOAISANPHAM loaiSanPham)
        {
            var loaiSanPhamInDb = db.TBL_LOAISANPHAM.Find(loaiSanPham.IDLOAI);

            if (loaiSanPhamInDb == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                // Cập nhật các thuộc tính
                loaiSanPhamInDb.TENLOAI = loaiSanPham.TENLOAI;
                loaiSanPhamInDb.HINHANH = loaiSanPham.HINHANH;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(loaiSanPham);
        }


        public ActionResult Details(int id)
        {
            TBL_LOAISANPHAM loaiSanPham = db.TBL_LOAISANPHAM.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        public ActionResult Delete(int id)
        {
            TBL_LOAISANPHAM loaiSanPham = db.TBL_LOAISANPHAM.Find(id);
            if (loaiSanPham != null)
            {
                db.TBL_LOAISANPHAM.Remove(loaiSanPham);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
