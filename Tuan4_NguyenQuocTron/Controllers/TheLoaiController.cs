using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuan4_NguyenQuocTron.Models;

namespace Tuan4_NguyenQuocTron.Controllers
{
    public class TheLoaiController : Controller
    {
        MyDataDataContext Mydata = new MyDataDataContext();
        // GET: TheLoai
        public ActionResult Index()
        {
            var all_theloai = from tt in Mydata.TheLoais select tt;
            return View(all_theloai);
        }
        //-------------Create-------------------
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, TheLoai tl)
        {
            var ten = collection["tenloai"];
            if (string.IsNullOrEmpty(ten))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                tl.tenloai = ten;
                Mydata.TheLoais.InsertOnSubmit(tl);
                Mydata.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }
    }
}