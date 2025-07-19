using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoHoangNhan_2211110151_DeAnWeb.Models
{
    public class giohang
    {
        public List<sanpham_tronggiohang> SP;
        public float TONGTIEN;
        public giohang()
        {
            SP = new List<sanpham_tronggiohang>();
            TONGTIEN = 0; ;
        }
        public giohang(List<sanpham_tronggiohang> sP)
        {
            SP = sP;
            TONGTIEN = sP.Sum(t => t.THANHTIEN);
        }
        public void AddToGoHang(sanpham_tronggiohang obj)
        {
            SP.Add(obj);
        }
        public float GET_TONGTIEN()
        {
            return SP.Sum(t => t.THANHTIEN);
        }
        public void UpdateItem(int id, int slnew)
        {
            sanpham_tronggiohang spnew = SP.Where(t => t.ID == id).FirstOrDefault();
            if (spnew != null)
            {
                spnew.SO_LUONG = slnew;
                spnew.THANHTIEN = spnew.GIATIEN * slnew;
                int vt = SP.IndexOf(spnew);
                if (vt > -1)
                {
                    SP[vt] = spnew;
                }
            }
        }
    }
}