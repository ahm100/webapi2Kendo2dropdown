using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEshop.Classes;
using MyEshop.Models;

namespace MyEshop.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        private Mvc_Eshop_DBEntities db = new Mvc_Eshop_DBEntities();

        // GET: Admin/Pages
        public ActionResult Index()
        {
            return View(db.Pages.ToList());
        }

        // GET: Admin/Pages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pages pages = db.Pages.Find(id);
            if (pages == null)
            {
                return HttpNotFound();
            }
            return View(pages);
        }

        // GET: Admin/Pages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PageID,PageTitle,ShortDescription,PageText,PageSee,PageImage")] Pages pages, HttpPostedFileBase PageImage)
        {
            if (ModelState.IsValid)
            {
                if (PageImage != null)
                {
                    if (CheckContentImage.IsImage(PageImage))
                    {
                        string imagename = Guid.NewGuid().ToString() + Path.GetExtension(PageImage.FileName);
                        PageImage.SaveAs(Server.MapPath("/Images/PageImages/" + imagename));
                        pages.PageImage = imagename;
                    }
                    else
                    {
                        ModelState.AddModelError("PageImage", "تصویر معتبر نیست");
                        return View(pages);
                    }
                }

                pages.PageSee = 0;
                db.Pages.Add(pages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pages);
        }

        // GET: Admin/Pages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pages pages = db.Pages.Find(id);
            if (pages == null)
            {
                return HttpNotFound();
            }
            return View(pages);
        }

        // POST: Admin/Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PageID,PageTitle,ShortDescription,PageText,PageSee,PageImage")] Pages pages, HttpPostedFileBase PageImages)
        {
            if (ModelState.IsValid)
            {

                if (PageImages != null)
                {
                    if (CheckContentImage.IsImage(PageImages))
                    {

                        if (pages.PageImage != null)
                        {
                            System.IO.File.Delete(Server.MapPath("/Images/PageImages/" + pages.PageImage));
                        }


                        string imagename = Guid.NewGuid().ToString() + Path.GetExtension(PageImages.FileName);
                        PageImages.SaveAs(Server.MapPath("/Images/PageImages/" + imagename));
                        pages.PageImage = imagename;
                    }
                    else
                    {
                        ModelState.AddModelError("PageImage", "تصویر معتبر نیست");
                        return View(pages);
                    }
                }

                db.Entry(pages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pages);
        }

        // GET: Admin/Pages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pages pages = db.Pages.Find(id);
            if (pages == null)
            {
                return HttpNotFound();
            }
            return View(pages);
        }

        // POST: Admin/Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pages pages = db.Pages.Find(id);
            if (pages.PageImage != null)
            {
                System.IO.File.Delete(Server.MapPath("/Images/PageImages/" + pages.PageImage));
            }
            db.Pages.Remove(pages);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
