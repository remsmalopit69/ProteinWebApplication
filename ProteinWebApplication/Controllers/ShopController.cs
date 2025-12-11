using ProteinWebApplication.Models;
using ProteinWebApplication.Models.Context;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ProteinWebApplication.Controllers
{
    public class ShopController : Controller
    {
        // ==================== PUBLIC VIEWS ====================
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult Categories()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult ProductDetails(int id)
        {
            ViewBag.ProductID = id;
            return View();
        }

        // ==================== API ENDPOINTS FOR FRONTEND ====================
        public JsonResult GetAllProducts()
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var products = db.tbl_products
                        .Where(x => x.isArchive == 0)
                        .OrderBy(x => x.displayOrder)
                        .Select(p => new
                        {
                            p.productID,
                            p.productName,
                            p.productDescription,
                            p.price,
                            p.stockQuantity,
                            p.categoryID,
                            categoryName = db.tbl_categories
                                .Where(c => c.categoryID == p.categoryID)
                                .Select(c => c.categoryName)
                                .FirstOrDefault(),
                            image = db.tbl_images
                                .Where(i => i.referenceID == p.productID && i.imageType == "product" && i.isArchive == 0)
                                .OrderBy(i => i.displayOrder)
                                .Select(i => i.imagePath)
                                .FirstOrDefault()
                        })
                        .ToList();
                    return Json(products, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllCategories()
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var categories = db.tbl_categories
                        .Where(x => x.isArchive == 0)
                        .OrderBy(x => x.displayOrder)
                        .Select(c => new
                        {
                            c.categoryID,
                            c.categoryName,
                            c.categoryDescription,
                            productCount = db.tbl_products.Count(p => p.categoryID == c.categoryID && p.isArchive == 0),
                            image = db.tbl_images
                                .Where(i => i.referenceID == c.categoryID && i.imageType == "category" && i.isArchive == 0)
                                .OrderBy(i => i.displayOrder)
                                .Select(i => i.imagePath)
                                .FirstOrDefault()
                        })
                        .ToList();
                    return Json(categories, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProductsByCategory(int categoryID)
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var products = db.tbl_products
                        .Where(x => x.categoryID == categoryID && x.isArchive == 0)
                        .OrderBy(x => x.displayOrder)
                        .Select(p => new
                        {
                            p.productID,
                            p.productName,
                            p.productDescription,
                            p.price,
                            p.stockQuantity,
                            p.categoryID,
                            image = db.tbl_images
                                .Where(i => i.referenceID == p.productID && i.imageType == "product" && i.isArchive == 0)
                                .OrderBy(i => i.displayOrder)
                                .Select(i => i.imagePath)
                                .FirstOrDefault()
                        })
                        .ToList();
                    return Json(products, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProductDetails(int productID)
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var product = db.tbl_products
                        .Where(x => x.productID == productID && x.isArchive == 0)
                        .Select(p => new
                        {
                            p.productID,
                            p.productName,
                            p.productDescription,
                            p.price,
                            p.stockQuantity,
                            p.categoryID,
                            categoryName = db.tbl_categories
                                .Where(c => c.categoryID == p.categoryID)
                                .Select(c => c.categoryName)
                                .FirstOrDefault(),
                            images = db.tbl_images
                                .Where(i => i.referenceID == p.productID && i.imageType == "product" && i.isArchive == 0)
                                .OrderBy(i => i.displayOrder)
                                .Select(i => new { i.imageID, i.imagePath, i.imageName })
                                .ToList()
                        })
                        .FirstOrDefault();
                    return Json(product, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBannerImages()
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var banners = db.tbl_images
                        .Where(x => x.imageType == "banner" && x.isArchive == 0)
                        .OrderBy(x => x.displayOrder)
                        .Select(i => new { i.imageID, i.imagePath, i.imageName })
                        .ToList();
                    return Json(banners, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetHeroImage()
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var hero = db.tbl_images
                        .Where(x => x.imageType == "hero" && x.isArchive == 0)
                        .OrderByDescending(x => x.createdAt)
                        .Select(i => new { i.imageID, i.imagePath, i.imageName })
                        .FirstOrDefault();
                    return Json(hero, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}