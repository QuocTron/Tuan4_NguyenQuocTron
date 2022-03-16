using System;
using System.Collections.Generic;
using System.Linq;
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
            List< GioHang > lstGiohang = Laygiohang();
          
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
                    List < GioHang > lstGiohang = Session["GioHang"] as List<GioHang>;
                    if (lstGiohang != null) 
                    {
                        tsl = lstGiohang.Sum(n => n.isoluong);
                    }
                    return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0; 
            List < GioHang > lstGiohang = Session["GioHang"] as List<GioHang>; 
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
            ViewBag.Tongsoluongsanpham =TongSoLuongSanPham();
            return View(lstGiohang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong(); 
            ViewBag.Tongtien = TongTien(); 
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham(); 
            return PartialView();

        }
        public ActionResult XoaGiohang(int id)
        {
            List < GioHang > lstGiohang = Laygiohang(); 
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
            
            List < GioHang > lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.masach == id);
            Sach sach = data.Saches.SingleOrDefault(n => n.masach == id);
            if (sach.soluongton < sanpham.isoluong)
            {
                return PartialView("ThongBao");
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
        public ActionResult DatHang()
        {
            // Kiểm tra Session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<GioHang> listGH = Laygiohang(); // nhận giá trị từ session
                foreach (var item in listGH)
                {
                    Sach sach = data.Saches.FirstOrDefault(p => p.masach == item.masach);
                    sach.soluongton -= item.isoluong;
                    UpdateModel(sach);
                    data.SubmitChanges();
                }
            }
            Session["GioHang"] = null;
            return RedirectToAction("GioHang");
        }
        
    }
}
    
    

        
       

           
       
       

           
      



