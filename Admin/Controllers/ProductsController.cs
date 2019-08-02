using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using InsertShowImage;
using MyEshop.Areas.Admin.Models;
using MyEshop.Models;

namespace MyEshop.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private Mvc_Eshop_DBEntities db = new Mvc_Eshop_DBEntities();

        // GET: Admin/Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Product_Groups);
            return View(products.ToList());
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            Session["Features"] = null;
            ViewBag.GroupID = new SelectList(db.Product_Groups, "GroupID", "GroupTitle");
            ViewBag.FeatureID = new SelectList(db.Features, "FeatureID", "FeatureTitle");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products, string Tags, HttpPostedFileBase ImageUp, HttpPostedFileBase[] GalleryUp)
        {
            if (ModelState.IsValid)
            {
                string imageName = "no-photo.jpg";

                if (ImageUp != null)
                {
                    imageName = ImageUp.FileName;

                    ImageUp.SaveAs(Server.MapPath("/Images/ProductImages/Image/" + imageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/ProductImages/Image/" + imageName),
                        Server.MapPath("/Images/ProductImages/Thumb/" + imageName));
                }

                products.ProductImage = imageName;
                db.Products.Add(products);


                #region Tags

                string[] Tag = Tags.Split('-');
                foreach (string t in Tag)
                {
                    string tagInsert = t.Trim();
                    while (tagInsert.Contains("  "))
                    {
                        tagInsert = tagInsert.Replace("  ", " ");
                    }
                    db.Product_Tags.Add(new Product_Tags()
                    {
                        ProductID = products.ProductID,
                        TagTitle = tagInsert
                    });
                }

                #endregion
                db.SaveChanges();
                #region Gallery

                if (GalleryUp != null && GalleryUp.Any())
                {
                    foreach (HttpPostedFileBase file in GalleryUp)
                    {
                        string name =products.ProductID+"_"+ file.FileName;

                        file.SaveAs(Server.MapPath("/Images/ProductImages/Image/"+name));

                        ImageResizer img=new ImageResizer();
                        img.Resize(Server.MapPath("/Images/ProductImages/Image/" + name),
                            Server.MapPath("/Images/ProductImages/Thumb/" + name));

                        db.Product_Galleries.Add(new Product_Galleries()
                        {
                            ProductID = products.ProductID,
                            ImageName = name
                        });
                    }
                }

            #endregion

#region Features
                if (Session["Features"] != null)
                {
                    List<FeatureListItemViewModel> list = Session["Features"] as List<FeatureListItemViewModel>;

                    foreach (var feature in list)
                    {
                        db.Product_Features.Add(new Product_Features()
                        {
                            FeatureID = feature.FeatureID,
                            ProductID = products.ProductID,
                            FeatureValue = feature.FeatureValue
                        });
                    }
                }
#endregion
                db.SaveChanges();
                Session["Features"] = null;
                return RedirectToAction("Index");
            }

            ViewBag.GroupID = new SelectList(db.Product_Groups, "GroupID", "GroupTitle", products.GroupID);
            return View(products);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            Session["Features"] = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.FeatureID = new SelectList(db.Features, "FeatureID", "FeatureTitle");
            ViewBag.GroupID = new SelectList(db.Product_Groups, "GroupID", "GroupTitle", products.GroupID);
            ViewBag.Tags = string.Join("-", products.Product_Tags.Select(t => t.TagTitle).ToList());
            Session["Features"] = products.Product_Features.Select(s => new FeatureListItemViewModel()
            {
                FeatureValue = s.FeatureValue,
                FeatureID = s.FeatureID,
                Title = s.Features.FeatureTitle
            }).ToList();

            return View(products);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products, string Tags, HttpPostedFileBase ImageUp, HttpPostedFileBase[] GalleryUp)
        {
            if (ModelState.IsValid)
            {
                if (ImageUp != null)
                {
                    if (products.ProductImage != "no-photo.jpg")
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Thumb/" + products.ProductImage));
                        System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Image/" + products.ProductImage));
                    }

                    products.ProductImage = ImageUp.FileName;

                    ImageUp.SaveAs(Server.MapPath("/Images/ProductImages/Image/" + products.ProductImage));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/ProductImages/Image/" + products.ProductImage),
                        Server.MapPath("/Images/ProductImages/Thumb/" + products.ProductImage));
                }

                db.Entry(products).State = EntityState.Modified;

                db.Product_Tags.Where(t=>t.ProductID==products.ProductID).ToList().ForEach(t=>db.Product_Tags.Remove(t));

                #region Tags

                string[] Tag = Tags.Split('-');
                foreach (string t in Tag)
                {
                    string tagInsert = t.Trim();
                    while (tagInsert.Contains("  "))
                    {
                        tagInsert = tagInsert.Replace("  ", " ");
                    }
                    db.Product_Tags.Add(new Product_Tags()
                    {
                        ProductID = products.ProductID,
                        TagTitle = tagInsert
                    });
                }

                #endregion
                #region Gallery

                if (GalleryUp != null && GalleryUp.Any())
                {
                    foreach (HttpPostedFileBase file in GalleryUp)
                    {
                        string name = products.ProductID + "_" + file.FileName;

                        file.SaveAs(Server.MapPath("/Images/ProductImages/Image/" + name));

                        ImageResizer img = new ImageResizer();
                        img.Resize(Server.MapPath("/Images/ProductImages/Image/" + name),
                            Server.MapPath("/Images/ProductImages/Thumb/" + name));

                        db.Product_Galleries.Add(new Product_Galleries()
                        {
                            ProductID = products.ProductID,
                            ImageName = name
                        });
                    }
                }

                #endregion


                #region Features

                db.Product_Features.Where(t => t.ProductID == products.ProductID).ToList().ForEach(t => db.Product_Features.Remove(t));

                if (Session["Features"] != null)
                {
                    List<FeatureListItemViewModel> list = Session["Features"] as List<FeatureListItemViewModel>;

                    foreach (var feature in list)
                    {
                        db.Product_Features.Add(new Product_Features()
                        {
                            FeatureID = feature.FeatureID,
                            ProductID = products.ProductID,
                            FeatureValue = feature.FeatureValue
                        });
                    }
                }
                #endregion
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupID = new SelectList(db.Product_Groups, "GroupID", "GroupTitle", products.GroupID);
            return View(products);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Product_Tags.Where(t => t.ProductID == products.ProductID).ToList().ForEach(t => db.Product_Tags.Remove(t));
            if (products.ProductImage != "no-photo.jpg")
            {
                System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Thumb/" + products.ProductImage));
                System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Image/" + products.ProductImage));
            }
            foreach (var gallery in db.Product_Galleries.Where(g=>g.ProductID==products.ProductID))
            {
                System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Thumb/" + gallery.ImageName));
                System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Image/" + gallery.ImageName));
                db.Product_Galleries.Remove(gallery);
            }
            db.Product_Features.Where(t => t.ProductID == products.ProductID).ToList().ForEach(t => db.Product_Features.Remove(t));
            db.Products.Remove(products);
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

        public void DeleteImage(int id)
        {
            var gallery = db.Product_Galleries.Find(id);

            System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Thumb/"+gallery.ImageName));
            System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Image/"+gallery.ImageName));
            db.Product_Galleries.Remove(gallery);
            db.SaveChanges();
        }

        public ActionResult AddFeature(int FeatureID, string Title, string Value)
        {
            List<FeatureListItemViewModel> list=new List<FeatureListItemViewModel>();

            if (Session["Features"] != null)
            {
                list = Session["Features"] as List<FeatureListItemViewModel>;
            }


            list.Add(new FeatureListItemViewModel()
            {
                FeatureID = FeatureID,
                Title = Title,
                FeatureValue = Value
            });

            Session["Features"] = list;

            return PartialView("ListFeatures", list);
        }

        public ActionResult DeleteFeature(int featureId, string value)
        {
            List<FeatureListItemViewModel> list = new List<FeatureListItemViewModel>();

            if (Session["Features"] != null)
            {
                list = Session["Features"] as List<FeatureListItemViewModel>;

                int index = list.FindIndex(f => f.FeatureID == featureId && f.FeatureValue == value);
                list.RemoveAt(index);

                Session["Features"] = list;
            }


            return PartialView("ListFeatures", list);

        }
    }
}
