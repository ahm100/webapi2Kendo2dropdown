using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEshop.Classes;
using MyEshop.Models;

namespace MyEshop.Areas.Admin.Controllers
{
    public class SlidersController : Controller
    {
        private Mvc_Eshop_DBEntities db = new Mvc_Eshop_DBEntities();

        // GET: Admin/Sliders
        public ActionResult Index()
        {
            return View(db.Slider.ToList());
        }

        // GET: Admin/Sliders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: Admin/Sliders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sliders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SlideID,SlideTitle,SlideImage,ClickCount,SlideLink,IsActive")] Slider slider, HttpPostedFileBase ImageUp)
        {
            if (ModelState.IsValid)
            {
                if (ImageUp != null)
                {

                    if (CheckContentImage.IsImage(ImageUp))
                    {

                        
                        slider.ClickCount = 0;
                        slider.SlideImage = "-";
                        db.Slider.Add(slider);
                        db.SaveChanges();
                      
                        slider.SlideImage = slider.SlideID+"_"+ImageUp.FileName;
                        ImageUp.SaveAs(Server.MapPath("/Images/SlideImages/" + slider.SlideImage));

                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("SlideImage", "تصویر معتبر نیست");
                    }
                }
                else
                {
                    ModelState.AddModelError("SlideImage","تصویر را انتخاب کنید");
                }
            }

            return View(slider);
        }

        // GET: Admin/Sliders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/Sliders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SlideID,SlideTitle,SlideImage,ClickCount,SlideLink,IsActive")] Slider slider, HttpPostedFileBase ImageUp)
        {
            if (ModelState.IsValid)
            {
                if (ImageUp != null)
                {
                    if (CheckContentImage.IsImage(ImageUp))
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/SlideImages/"+slider.SlideImage));

                        slider.SlideImage = slider.SlideID + "_" + ImageUp.FileName;
                        ImageUp.SaveAs(Server.MapPath("/Images/SlideImages/"+slider.SlideImage));
                    }
                    else
                    {
                        ModelState.AddModelError("SlideImage", "تصویر معتبر نیست");
                        return View(slider);
                    }
                }

               
                        db.Entry(slider).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                  
                
               
            }
            return View(slider);
        }

        // GET: Admin/Sliders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = db.Slider.Find(id);
            System.IO.File.Delete(Server.MapPath("/Images/SlideImages/" + slider.SlideImage));
            db.Slider.Remove(slider);
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
