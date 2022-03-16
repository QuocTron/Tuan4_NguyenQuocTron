using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuan4_NguyenQuocTron.Models;
using PagedList;

namespace Tuan4_PhanHoangHac.Controllers
{
    public class SachController : Controller
    {
        MyDataDataContext Mydata = new MyDataDataContext();
        // GET: Sach
        public ActionResult ListSach()
        {
            var listsach = from ls in Mydata.Saches select ls;
            return View(listsach);
        }

        // GET: Sach/Details/5
        public ActionResult Details(int id)
        {
            var detail = Mydata.Saches.Where(p => p.masach == id).First();
            return View(detail);
        }

        // GET: Sach/Create
        public ActionResult Create()
        {
            var listTheLoai = from th in Mydata.TheLoais select th;
            SelectList listTest = new SelectList(listTheLoai, "maloai", "tenloai");
            ViewBag.TheLoai = listTest;
            return View();
        }

        // POST: Sach/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, Sach s, string maLoai)
        {
            try
            {
                var E_loaisach = collection["maLoai"];
                var E_tensach = collection["tensach"];
                var E_hinh = collection["hinh"];
                var E_giaban = Convert.ToDecimal(collection["giaban"]);
                var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]); 
                var E_soluongton = Convert.ToInt32(collection["soluongton"]); 
                if (string.IsNullOrEmpty(E_tensach))
                {
                    ViewData["Error"] = "Don't empty!";
                }
                else
                {
                    s.maloai =int.Parse(E_loaisach);
                    s.tensach = E_tensach.ToString();
                    s.hinh = E_hinh.ToString();
                    s.giaban = E_giaban;
                    s.ngaycapnhat = E_ngaycapnhat;
                    s.soluongton = E_soluongton;
                    Mydata.Saches.InsertOnSubmit(s);
                    Mydata.SubmitChanges();
                    return RedirectToAction("ListSach");
                }
                return this.Create();

            }
            catch
            {
                return View();
            }
        }

        // GET: Sach/Edit/5
        public ActionResult Edit(int id)
        {
            var edit = Mydata.Saches.Where(p => p.masach == id).First();
            var listTheLoai = from th in Mydata.TheLoais select th;
            ViewBag.TheLoai = listTheLoai;
            return View(edit);
        }

        // POST: Sach/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
             
                var E_sach = Mydata.Saches.First(m => m.masach == id);
                var E_tensach = collection["tensach"];
                var E_maloai = collection["loaiSach"];
                var E_hinh = collection["hinh"];
                var E_giaban = Convert.ToDecimal(collection["giaban"]); var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycatnhat"]); var E_soluongton = Convert.ToInt32(collection["soluongton"]); E_sach.masach = id;
                if (string.IsNullOrEmpty(E_tensach))
                {
                    ViewData["Error"] = "Don't empty!";
                }
                else
                {
                    E_sach.maloai = int.Parse(E_maloai);
                    E_sach.tensach = E_tensach;
                    E_sach.hinh = E_hinh;
                    E_sach.giaban = E_giaban;
                    E_sach.ngaycapnhat = E_ngaycapnhat;
                    E_sach.soluongton = E_soluongton;
                    UpdateModel(E_sach);
                    Mydata.SubmitChanges();
                    return RedirectToAction("ListSach");
                }
                return this.Edit(id);
            }
            catch
            {
                return View();
            }
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }


        // GET: Sach/Delete/5
        public ActionResult Delete(int id)
        {
            var D_sach = Mydata.Saches.First(m => m.masach == id);
            return View(D_sach);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_sach = Mydata.Saches.Where(m => m.masach == id).First();
            Mydata.Saches.DeleteOnSubmit(D_sach);
            Mydata.SubmitChanges();
            return RedirectToAction("ListSach");
        }
    }
}
