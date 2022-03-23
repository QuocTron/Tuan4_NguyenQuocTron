using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Tuan4_NguyenQuocTron.Models;

namespace Tuan4_NguyenQuocTron.Controllers
{
    public class GioHangController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<GioHang>();
                Session["GioHang"] = lstGiohang;
            }
            return lstGiohang;
        }



        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<GioHang> lstGiohang = Laygiohang();

            GioHang sanpham = lstGiohang.Find(n => n.masach == id);
            Sach sach = data.Saches.SingleOrDefault(n => n.masach == id);
            if (sach.soluongton < 1)
            {
                return View("ThongBao");
            }
            if (sanpham == null)
            {
                sanpham = new GioHang(id);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.isoluong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Sum(n => n.isoluong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {

                tsl = lstGiohang.Count;
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
                tt = lstGiohang.Sum(n => n.dThanhtien);
            return tt;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            ViewBag.Tile = null;
            return View(lstGiohang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();

        }
        public ActionResult XoaGiohang(int? id)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.masach == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {

            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.masach == id);
            Sach sach = data.Saches.SingleOrDefault(n => n.masach == id);
            int slSanPham = int.Parse(collection["txtSolg"].ToString());
            if (sach.soluongton < slSanPham)
            {
                return View("ThongBao");
            }
            if (sanpham != null)
            {
                sanpham.isoluong = int.Parse(collection["txtSolg"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");


        }
        //public ActionResult DatHang()
        //{
        //    // Kiểm tra Session giỏ hàng tồn tại chưa
        //    if (Session["GioHang"] == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        List<GioHang> listGH = Laygiohang(); // nhận giá trị từ session
        //        foreach (var item in listGH)
        //        {
        //            Sach sach = data.Saches.FirstOrDefault(p => p.masach == item.masach);
        //            sach.soluongton -= item.isoluong;
        //            UpdateModel(sach);
        //            data.SubmitChanges();
        //        }
        //    }
        //    Session["GioHang"] = null;
        //    ViewBag.Tile = "Đã đặt hàng";
        //    return RedirectToAction("GioHang");
        //}
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Sach");
            }
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Sach s = new Sach();
            List<GioHang> gh = Laygiohang();
            var emailClient = collection["emailClient"];
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;
            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.masach = item.masach;
                ctdh.soluong = item.isoluong;
                ctdh.gia = (decimal)item.giaban;
                s = data.Saches.Single(n => n.masach == item.masach);
                s.soluongton -= ctdh.soluong;
                data.SubmitChanges();
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
                data.SubmitChanges();
            }
            try
            {
                MailMessage mSG = new MailMessage();
                mSG.From = new MailAddress("quoctron.200901@gmail.com", "Hiệu sách của Trọn");
                mSG.To.Add(emailClient); // thêm địa chỉ mail người nhận
                mSG.Subject = "Thư cảm ơn của Trọn";// Thêm tiêu đề mail;
                mSG.Body = "Cảm ơn bạn đã mua sản phẩm bên chúng tôi chúng tôi sẽ giao hàng nhanh nhất có thể";
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential(ParameterPrivate.Email, ParameterPrivate.Password);
                smtp.Send(mSG);// gửi
                mSG = null;
            }catch(Exception ex)
            {
                Console.WriteLine("Lỗi chứng xác thực");
            }
            // Địa chỉ email và tên người gửi

            Session["GioHang"] = null;
            return RedirectToAction("XacnhanDonhang", "GioHang");
        }

        public ActionResult XacnhanDonHang()
        {
            return View();
        }
    }
}
    
    

        
       

           
       
       

           
      



