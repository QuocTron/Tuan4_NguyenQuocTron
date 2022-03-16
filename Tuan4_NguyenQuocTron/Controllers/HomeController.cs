using System;
using System.Collections.Generic;
using PagedList;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuan4_NguyenQuocTron.Models;

namespace Tuan4_NguyenQuocTron.Controllers
{
    public class HomeController : Controller
    {
        MyDataDataContext Mydata = new MyDataDataContext();
        public ActionResult Index(int ? page)
        {
            if (page == null) page = 1;

            var listSach = (from s in Mydata.Saches select s).OrderBy(m=>m.masach);
            int pageSize = 3;
            int pageNum = page ?? 1;
            return View(listSach.ToPagedList(pageNum,pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}