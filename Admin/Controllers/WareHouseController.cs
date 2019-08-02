using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEshop.Areas.Admin.Models;
using MyEshop.Classes;
using MyEshop.Models;

namespace MyEshop.Areas.Admin.Controllers
{
    public class WareHouseController : Controller
    {
        Mvc_Eshop_DBEntities db=new Mvc_Eshop_DBEntities();
        // GET: Admin/WareHouse
        public ActionResult Index()
        {
            List<WareHouseViewModel> list=new List<WareHouseViewModel>();


            foreach (var productse in db.Products)
            {
                list.Add(new WareHouseViewModel()
                {
                    ProductID = productse.ProductID,
                    ProductTitle = productse.Title,
                    Count = WareHouseChecker.ProductCheck(productse.ProductID)
                });
            }

            //return View(list.Where(w=>w.Count!=0).OrderByDescending(p=>p.Count));
            return View(list.OrderByDescending(p=>p.Count));
        }

        public ActionResult ListWareHouse(int id)
        {
            return View(db.WareHouse.Where(w => w.ProductID == id).OrderByDescending(p => p.date).ToList());
        }

        public ActionResult AddWareHouse(int id)
        {
            ViewBag.TypeID = new SelectList(db.WareHouseTypes, "TypeID", "TypeTitle");
            return PartialView(new WareHouse()
            {
                ProductID = id
            });
        }


        [HttpPost]
        public ActionResult AddWareHouse(WareHouse ware)
        {
        ware.date=DateTime.Now;
            db.WareHouse.Add(ware);
            db.SaveChanges();
            return RedirectToAction("ListWareHouse",new{id=ware.ProductID});
        }
    }
}