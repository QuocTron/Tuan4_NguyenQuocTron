﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Tuan4_NguyenQuocTron.Models;

namespace Tuan4_NguyenQuocTron.Controllers
{
    public class NguoiDungController : Controller{

    // GET: NguoiDung 
        MyDataDataContext Mydata = new MyDataDataContext();
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
    public ActionResult Dangky(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["hoten"]; 
            var tendangnhap = collection["tendangnhap"]; 
            var matkhau = collection["matkhau"]; 
            var MatKhauXacNhan = collection["MatKhauXacNhan"];
            var email = collection["email"];
            var diachi = collection["diachi"];  
            var dienthoai = collection["dienthoai"];
            var ngaysinhtest = collection["ngaysinh"];
            var ngaysinh =String.Format("{0:MM/dd/yyyy}", collection["ngaysinh"]);

            if (String.IsNullOrEmpty(MatKhauXacNhan))
            {
                ViewData["NhapMKXN"] = "Phải nhập mật khẩu xác nhận!";
            }
            else
            {
                if(!matkhau.Equals(MatKhauXacNhan)){
                    ViewData["MatKhauKhongGiongNhau"] = "Mật khẩu xác nhận phải giống nhau";
                }
                else
                {
                    kh.hoten = hoten; 
                    kh.tendangnhap = tendangnhap; 
                    kh.matkhau = matkhau; 
                    kh.email =email; 
                    kh.diachi = diachi; 
                    kh .dienthoai = dienthoai; 
                    kh.ngaysinh = DateTime.Parse(ngaysinh);

                    Mydata.KhachHangs.InsertOnSubmit(kh); 
                    Mydata.SubmitChanges();

                    return RedirectToAction("DangNhap");
                 
                }
            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            KhachHang kh = Mydata.KhachHangs.SingleOrDefault(n => n.tendangnhap == tendangnhap && n.matkhau == matkhau);
            if (kh != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                Session["TaiKhoan"] = kh;
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return RedirectToAction("Index", "Home");

        }
    }
}