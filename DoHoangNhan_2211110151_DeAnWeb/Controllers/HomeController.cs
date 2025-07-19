using DoHoangNhan_2211110151_DeAnWeb.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoHoangNhan_2211110151_DeAnWeb.Controllers
{
    public class HomeController : Controller
    {
        WEBDB_DOHOANGNHAN_2211110151Entities db = new WEBDB_DOHOANGNHAN_2211110151Entities();
        public ActionResult Index()
        {
            // Fetch product categories and pass them to the view via ViewBag
            var categories = db.TBL_LOAISANPHAM.ToList();
            ViewBag.Categories = categories;

            List<TBL_SANPHAM> lsanpham = db.TBL_SANPHAM.ToList();
            return View(lsanpham);
        }


        // Method trả về gợi ý sản phẩm theo từ khóa
        public JsonResult GetProductSuggestions(string keyword)
        {
            var suggestions = db.TBL_SANPHAM
                                .Where(p => p.TENSP.Contains(keyword))
                                .Select(p => new { p.IDSP, p.TENSP }) // Trả về ID và tên sản phẩm
                                .Take(5) // Giới hạn số lượng gợi ý (ở đây là 5)
                                .ToList();
            return Json(suggestions, JsonRequestBehavior.AllowGet);
        }



        public ActionResult LoaiSanPham()
        {
            var categories = db.TBL_LOAISANPHAM.ToList();
            ViewBag.Categories = categories;
            var danhSachLoaiSanPham = db.TBL_LOAISANPHAM.ToList(); // Lấy toàn bộ danh sách các loại sản phẩm
            return View(danhSachLoaiSanPham); // Truyền danh sách vào view
        }



        public ActionResult SanPham(string id)
        {
            var categories = db.TBL_LOAISANPHAM.ToList();
            ViewBag.Categories = categories;
            List<TBL_SANPHAM> lsanpham = db.TBL_SANPHAM.ToList();
            return View(lsanpham);
        }

        public ActionResult ChiTietSanPham(int id)
        {
            var categories = db.TBL_LOAISANPHAM.ToList();
            ViewBag.Categories = categories;
            TBL_SANPHAM sp = db.TBL_SANPHAM.Find(id);
            return View(sp);
        }

        public ActionResult AddToCarts(int id)
        {
            try
            {
                using (var db = new WEBDB_DOHOANGNHAN_2211110151Entities())
                {
                    giohang _giohang = Session["GIOHANG"] as giohang;

                    if (_giohang == null)
                    {
                        _giohang = new giohang();
                    }

                    sanpham_tronggiohang obj = _giohang.SP.FirstOrDefault(t => t.ID == id);

                    if (obj != null)
                    {
                        // Kiểm tra nếu số lượng hiện tại chưa đạt tới số lượng tồn trong kho
                        if (obj.SO_LUONG < obj.SO_LUONG_TON_TRONG_KHO)
                        {
                            _giohang.UpdateItem(id, (int)obj.SO_LUONG + 1);
                        }
                        else
                        {
                            var response = new { Code = 1, CountMatHang = _giohang.SP.Count, Msg = "Số lượng sản phẩm trong kho đã đạt giới hạn." };
                            return Json(response);
                        }
                    }
                    else
                    {
                        // Lấy thông tin sản phẩm từ bảng TBL_SANPHAM
                        TBL_SANPHAM sp = db.TBL_SANPHAM.Find(id);

                        if (sp != null)
                        {
                            obj = new sanpham_tronggiohang
                            {
                                ID = sp.IDSP,
                                MASP = sp.MASP,
                                TENSP = sp.TENSP,
                                GIATIEN = (float)sp.GIATIEN,
                                SO_LUONG = 1,
                                HINH = sp.HINH_DAI_DIEN,
                                THANHTIEN = (float)sp.GIATIEN,
                                THANG = (float)sp.THANG,
                                SO_LUONG_TON_TRONG_KHO = sp.SO_LUONG_TON_TRONG_KHO ?? 0 // Gán số lượng tồn kho
                            };
                            _giohang.AddToGoHang(obj);
                        }
                        else
                        {
                            var response = new { Code = 1, CountMatHang = 0, Msg = "Product not found" };
                            return Json(response);
                        }
                    }

                    _giohang.TONGTIEN = _giohang.SP.Sum(t => t.THANHTIEN);
                    Session["GIOHANG"] = _giohang;

                    var successResponse = new { Code = 0, CountMatHang = _giohang.SP.Count, Msg = "Success" };
                    return Json(successResponse);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new { Code = 1, CountMatHang = 0, Msg = ex.Message };
                return Json(errorResponse);
            }
        }


        [HttpPost]
        public JsonResult UpdateQuantity(int id, int quantity)
        {
            try
            {
                WEBDB_DOHOANGNHAN_2211110151Entities db = new WEBDB_DOHOANGNHAN_2211110151Entities();
                giohang _giohang = (giohang)Session["GIOHANG"];
                sanpham_tronggiohang obj = _giohang.SP.FirstOrDefault(t => t.ID == id);

                if (obj != null)
                {
                    obj.SO_LUONG = quantity;
                    obj.THANHTIEN = obj.GIATIEN * obj.SO_LUONG;
                    _giohang.TONGTIEN = _giohang.SP.Sum(t => t.THANHTIEN);
                    Session["GIOHANG"] = _giohang;
                    return Json(new { Code = 0, Msg = "Success" });
                }
                else
                {
                    return Json(new { Code = 1, Msg = "Product not found in cart" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = 1, Msg = ex.Message });
            }
        }


        public ActionResult ChiTietGioHang()
        {
            var categories = db.TBL_LOAISANPHAM.ToList();
            ViewBag.Categories = categories;
            giohang _giohang = (giohang)Session["GIOHANG"];
            return View(_giohang);
        }
        public ActionResult XoaSanPhamTrongGioHang(int id)
        {
            giohang _giohang = (giohang)Session["GIOHANG"];
            sanpham_tronggiohang obj = _giohang.SP.Where(t => t.ID == id).FirstOrDefault();
            if (obj != null)
            {
                _giohang.SP.Remove(obj);
                _giohang.TONGTIEN = _giohang.GET_TONGTIEN();
                Session["GIOHANG"] = _giohang;
                Session["SOLUONGMATHANG"] = _giohang.SP.Count;
            }
            string returnURL = Request.UrlReferrer.Segments[2];
            return RedirectToAction(returnURL);
        }

        public ActionResult ThanhToan()
        {
            var categories = db.TBL_LOAISANPHAM.ToList();
            ViewBag.Categories = categories;
            if (Session["UserEmail"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            giohang _giohang = (giohang)Session["GIOHANG"];
            ViewData["TONGTIEN"] = _giohang.TONGTIEN;
            ViewData["TENKHACHHANG"] = " ";
            ViewData["SODIENTHOAI"] = " ";
            ViewData["DIACHIEMAIL"] = " ";
            ViewData["GHICHUDONHANG"] = " ";
            ViewData["TRANGTHAITHANHTOAN"] = " ";
            return View(_giohang);
        }

        [HttpPost]
        public ActionResult ThanhToan(FormCollection form)
        {
            if (Session["UserEmail"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            string email = Session["UserEmail"].ToString();
            string hoTenKH = form["TENKHACHHANG"];
            string sdt = form["SODIENTHOAI"];
            string emailInput = form["DIACHIEMAIL"];
            string ghiChu = form["GHICHUDONHANG"];

            giohang _giohang = (giohang)Session["GIOHANG"];

            if (_giohang == null || _giohang.SP.Count == 0)
            {
                ViewData["TRANGTHAITHANHTOAN"] = "BẠN CHƯA CÓ SẢN PHẨM NÀO TRONG GIỎ HÀNG";
                return View(_giohang);
            }

            using (var db = new WEBDB_DOHOANGNHAN_2211110151Entities())
            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var hoadon = new TBL_HOADON
                    {
                        NGAYBAN = DateTime.Now,
                        TEN_KHACH_HANG = hoTenKH,
                        SDT = sdt,
                        EMAIL = email,
                        GHICHU = ghiChu,
                        TONG_THANH_TIEN = _giohang.TONGTIEN,
                        MAHD = DateTime.Now.ToString().Replace(" ", "_").Replace(":", "_").Replace("/", "_"),
                        TRANG_THAI = 1
                    };
                    db.TBL_HOADON.Add(hoadon);
                    db.SaveChanges();

                    string _MAHD = hoadon.MAHD;
                    foreach (var item in _giohang.SP)
                    {
                        // Tạo chi tiết hóa đơn
                        var chitiethd = new TBL_HOADON_CHITIET
                        {
                            MAHD = _MAHD,
                            MASP = item.MASP,
                            TENSP = item.TENSP,
                            SOLUONG = (int)item.SO_LUONG,
                            GIATIEN = item.GIATIEN,
                            THANH_TIEN = item.THANHTIEN
                        };
                        db.TBL_HOADON_CHITIET.Add(chitiethd);

                        // Cập nhật mã key sử dụng
                        var keysToUpdate = db.TBL_MAKEY
                                             .Where(k => k.MASP == item.MASP && k.IS_USED == false)
                                             .Take((int)item.SO_LUONG)
                                             .ToList();
                        foreach (var key in keysToUpdate)
                        {
                            key.IS_USED = true; // Đánh dấu mã key đã sử dụng
                        }

                        // Giảm số lượng tồn trong kho của sản phẩm
                        var sanpham = db.TBL_SANPHAM.SingleOrDefault(sp => sp.MASP == item.MASP);
                        if (sanpham != null)
                        {
                            sanpham.SO_LUONG_TON_TRONG_KHO -= (int)item.SO_LUONG;
                            if (sanpham.SO_LUONG_TON_TRONG_KHO < 0) sanpham.SO_LUONG_TON_TRONG_KHO = 0; // Đảm bảo không bị âm
                        }
                    }


                    db.SaveChanges();
                    tran.Commit();
                    _giohang = new giohang(); // Clear cart after successful transaction
                    return RedirectToAction("ThanhToanThanhCong");
                }
                catch (DbEntityValidationException dbEx)
                {
                    tran.Rollback();
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ViewData["TRANGTHAITHANHTOAN"] += $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}\n";
                        }
                    }
                    return View(_giohang);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    ViewData["TRANGTHAITHANHTOAN"] = ": " + ex.Message;
                    return View(_giohang);
                }
            }
        }




        public ActionResult ThanhToanThanhCong()
        {
            var categories = db.TBL_LOAISANPHAM.ToList();
            ViewBag.Categories = categories;
            if (Session["UserEmail"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            string email = Session["UserEmail"].ToString();
            using (var db = new WEBDB_DOHOANGNHAN_2211110151Entities())
            {
                var ds_hoadon = db.TBL_HOADON.Where(hd => hd.EMAIL == email && hd.TRANG_THAI == 1).ToList();
                if (ds_hoadon.Count == 0)
                {
                    return RedirectToAction("Index");
                }
                return View(ds_hoadon);
            }
        }





        






        private Payment payment;
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            // Lấy APIContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    // Tạo Payment nếu không có PayerID (người dùng chưa xác nhận thanh toán)
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/PaymentWithPayPal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // Thực thi thanh toán khi có PayerID từ PayPal
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }

            return RedirectToAction("ThanhToanThanhCong");
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };

            this.payment = new Payment()
            {
                id = paymentId
            };

            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            giohang _giohang = (giohang)Session["GIOHANG"];
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            foreach (var item in _giohang.SP)
            {
                itemList.items.Add(new Item()
                {
                    name = item.TENSP,
                    currency = "USD",
                    price = item.GIATIEN.ToString(),
                    quantity = item.SO_LUONG.ToString(),
                    sku = item.MASP
                });
            }

            var payer = new Payer()
            {
                payment_method = "paypal"
            };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            float totalAmount = _giohang.TONGTIEN; // Tổng tiền của giỏ hàng

            var details = new Details()
            {
                tax = "0", // Thuế
                shipping = "0", // Phí vận chuyển
                subtotal = totalAmount.ToString() // Tổng tiền sản phẩm
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = totalAmount.ToString(), // Tổng tiền thanh toán
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Thanh toán đơn hàng",
                invoice_number = DateTime.Now.Ticks.ToString(), // Số hóa đơn (ví dụ)
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return this.payment.Create(apiContext);
        }



    }

}