using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoHoangNhan_2211110151_DeAnWeb.Models
{
    public class sanpham_tronggiohang
    {
        public int ID { get; set; }
        public string MASP { get; set; }
        public string TENSP { get; set; }
        public float GIATIEN { get; set; }
        public int SO_LUONG { get; set; }
        public string HINH { get; set; }
        public float THANHTIEN { get; set; }
        public float THANG { get; set; }

        // Thêm thuộc tính mới cho số lượng tồn trong kho
        public int SO_LUONG_TON_TRONG_KHO { get; set; }

        public sanpham_tronggiohang()
        {
            ID = -1;
            MASP = "";
            TENSP = "";
            GIATIEN = 0;
            SO_LUONG = 0;
            HINH = "";
            THANHTIEN = 0;
            THANG = 0;
            SO_LUONG_TON_TRONG_KHO = 0; // Khởi tạo giá trị mặc định cho số lượng tồn
        }

        public sanpham_tronggiohang(int iD, string mASP, string tENSP, float gIATIEN, int sO_LUONG, string hINH, float tHANG, int sO_LUONG_TON_TRONG_KHO)
        {
            ID = iD;
            MASP = mASP;
            TENSP = tENSP;
            GIATIEN = gIATIEN;
            SO_LUONG = sO_LUONG;
            HINH = hINH;
            THANHTIEN = sO_LUONG * gIATIEN;
            THANG = tHANG;
            SO_LUONG_TON_TRONG_KHO = sO_LUONG_TON_TRONG_KHO; // Gán giá trị từ tham số
        }
    }
}
