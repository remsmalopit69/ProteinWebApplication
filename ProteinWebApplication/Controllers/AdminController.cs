using ProteinWebApplication.Models;
using ProteinWebApplication.Models.Context;

using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProteinWebApplication.Controllers
{
    public class AdminController : Controller
    {
        // ==================== VIEWS ====================
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Categories()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult Images()
        {
            return View();
        }

        public ActionResult Orders()
        {
            return View();
        }

        public ActionResult Reports()
        {
            return View();
        }

        // ==================== AUTHENTICATION ====================
        public JsonResult LoginAdmin(string username, string password)
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var admin = db.tbl_admin_users
                        .Where(x => x.username == username && x.password == password && x.isArchive == 0)
                        .FirstOrDefault();

                    if (admin != null)
                    {
                        Session["AdminID"] = admin.adminID;
                        Session["AdminName"] = admin.fullName;
                        return Json(new { success = true, message = "Login successful" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Invalid credentials" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LogoutAdmin()
        {
            Session.Clear();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        // ==================== CATEGORIES CRUD ====================
        public JsonResult GetCategories()
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var categories = db.tbl_categories
                        .Where(x => x.isArchive == 0)
                        .OrderBy(x => x.displayOrder)
                        .ToList();
                    return Json(categories, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"An error occurred: {ex.Message} : {ex.InnerException} : {ex.StackTrace}");
            }
        }

        public JsonResult AddCategory(tblCategoriesModel category)
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    category.createdAt = DateTime.Now;
                    category.updatedAt = DateTime.Now;
                    category.isArchive = 0;
                    db.tbl_categories.Add(category);
                    db.SaveChanges();

                    var categories = db.tbl_categories.Where(x => x.isArchive == 0).ToList();
                    return Json(new { success = true, data = categories }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateCategory(tblCategoriesModel category)
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var existingCategory = db.tbl_categories
                        .Where(x => x.categoryID == category.categoryID)
                        .FirstOrDefault();

                    if (existingCategory != null)
                    {
                        existingCategory.categoryName = category.categoryName;
                        existingCategory.categoryDescription = category.categoryDescription;
                        existingCategory.displayOrder = category.displayOrder;
                        existingCategory.updatedAt = DateTime.Now;
                        db.SaveChanges();

                        var categories = db.tbl_categories.Where(x => x.isArchive == 0).ToList();
                        return Json(new { success = true, data = categories }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "Category not found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ArchiveCategory(int categoryID)
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var category = db.tbl_categories
                        .Where(x => x.categoryID == categoryID)
                        .FirstOrDefault();

                    if (category != null)
                    {
                        category.isArchive = 1;
                        db.SaveChanges();

                        var categories = db.tbl_categories.Where(x => x.isArchive == 0).ToList();
                        return Json(new { success = true, data = categories }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "Category not found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // ==================== PRODUCTS CRUD ====================
        public JsonResult GetProducts()
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var products = db.tbl_products
                        .Where(x => x.isArchive == 0)
                        .OrderBy(x => x.displayOrder)
                        .ToList();
                    return Json(products, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"An error occurred: {ex.Message} : {ex.InnerException} : {ex.StackTrace}");
            }
        }

        public JsonResult AddProduct(tblProductsModel product)
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    product.createdAt = DateTime.Now;
                    product.updatedAt = DateTime.Now;
                    product.isArchive = 0;
                    db.tbl_products.Add(product);
                    db.SaveChanges();

                    var products = db.tbl_products.Where(x => x.isArchive == 0).ToList();
                    return Json(new { success = true, data = products }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateProduct(tblProductsModel product)
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var existingProduct = db.tbl_products
                        .Where(x => x.productID == product.productID)
                        .FirstOrDefault();

                    if (existingProduct != null)
                    {
                        existingProduct.categoryID = product.categoryID;
                        existingProduct.productName = product.productName;
                        existingProduct.productDescription = product.productDescription;
                        existingProduct.price = product.price;
                        existingProduct.stockQuantity = product.stockQuantity;
                        existingProduct.displayOrder = product.displayOrder;
                        existingProduct.updatedAt = DateTime.Now;
                        db.SaveChanges();

                        var products = db.tbl_products.Where(x => x.isArchive == 0).ToList();
                        return Json(new { success = true, data = products }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "Product not found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ArchiveProduct(int productID)
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var product = db.tbl_products
                        .Where(x => x.productID == productID)
                        .FirstOrDefault();

                    if (product != null)
                    {
                        product.isArchive = 1;
                        db.SaveChanges();

                        var products = db.tbl_products.Where(x => x.isArchive == 0).ToList();
                        return Json(new { success = true, data = products }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "Product not found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // ==================== IMAGES CRUD ====================
        public JsonResult GetImages()
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var images = db.tbl_images
                        .Where(x => x.isArchive == 0)
                        .OrderBy(x => x.displayOrder)
                        .ToList();
                    return Json(images, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"An error occurred: {ex.Message} : {ex.InnerException} : {ex.StackTrace}");
            }
        }

        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileBase imageFile, string imageType, int? referenceID, int displayOrder)
        {
            try
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(imageFile.FileName);
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                    string uploadPath = Server.MapPath("~/Content/Images/");

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string filePath = Path.Combine(uploadPath, uniqueFileName);
                    imageFile.SaveAs(filePath);

                    using (var db = new ProteinContext())
                    {
                        var image = new tblImagesModel
                        {
                            imageName = fileName,
                            imagePath = "/Content/Images/" + uniqueFileName,
                            imageType = imageType,
                            referenceID = referenceID,
                            displayOrder = displayOrder,
                            createdAt = DateTime.Now,
                            updatedAt = DateTime.Now,
                            isArchive = 0
                        };
                        db.tbl_images.Add(image);
                        db.SaveChanges();

                        var images = db.tbl_images.Where(x => x.isArchive == 0).ToList();
                        return Json(new { success = true, data = images }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = false, message = "No file uploaded" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ArchiveImage(int imageID)
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var image = db.tbl_images
                        .Where(x => x.imageID == imageID)
                        .FirstOrDefault();

                    if (image != null)
                    {
                        image.isArchive = 1;
                        db.SaveChanges();

                        var images = db.tbl_images.Where(x => x.isArchive == 0).ToList();
                        return Json(new { success = true, data = images }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "Image not found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // ==================== ORDERS CRUD ====================
        public JsonResult GetOrders()
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var orders = db.tbl_orders
                        .Where(x => x.isArchive == 0)
                        .OrderByDescending(x => x.orderDate)
                        .ToList();
                    return Json(orders, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"An error occurred: {ex.Message} : {ex.InnerException} : {ex.StackTrace}");
            }
        }

        public JsonResult UpdateOrderStatus(int orderID, string orderStatus)
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var order = db.tbl_orders
                        .Where(x => x.orderID == orderID)
                        .FirstOrDefault();

                    if (order != null)
                    {
                        order.orderStatus = orderStatus;
                        order.updatedAt = DateTime.Now;
                        db.SaveChanges();

                        var orders = db.tbl_orders.Where(x => x.isArchive == 0).ToList();
                        return Json(new { success = true, data = orders }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "Order not found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ArchiveOrder(int orderID)
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var order = db.tbl_orders
                        .Where(x => x.orderID == orderID)
                        .FirstOrDefault();

                    if (order != null)
                    {
                        order.isArchive = 1;
                        db.SaveChanges();

                        var orders = db.tbl_orders.Where(x => x.isArchive == 0).ToList();
                        return Json(new { success = true, data = orders }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "Order not found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // ==================== DASHBOARD ANALYTICS ====================
        public JsonResult GetDashboardStats()
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var totalProducts = db.tbl_products.Count(x => x.isArchive == 0);
                    var totalCategories = db.tbl_categories.Count(x => x.isArchive == 0);
                    var totalOrders = db.tbl_orders.Count(x => x.isArchive == 0);
                    var totalRevenue = db.tbl_orders
                        .Where(x => x.isArchive == 0)
                        .Sum(x => (decimal?)x.totalAmount) ?? 0;

                    return Json(new
                    {
                        totalProducts = totalProducts,
                        totalCategories = totalCategories,
                        totalOrders = totalOrders,
                        totalRevenue = totalRevenue
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSalesChartData()
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var salesData = db.tbl_analytics
                        .Where(x => x.metricType == "daily_sales" && x.isArchive == 0)
                        .OrderBy(x => x.recordDate)
                        .Take(7)
                        .Select(x => new
                        {
                            date = x.recordDate,
                            sales = x.metricValue
                        })
                        .ToList();

                    return Json(salesData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProductsByCategoryData()
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var categoryData = db.tbl_categories
                        .Where(x => x.isArchive == 0)
                        .Select(c => new
                        {
                            categoryName = c.categoryName,
                            productCount = db.tbl_products.Count(p => p.categoryID == c.categoryID && p.isArchive == 0)
                        })
                        .ToList();

                    return Json(categoryData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOrderStatusData()
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    var statusData = db.tbl_orders
                        .Where(x => x.isArchive == 0)
                        .GroupBy(x => x.orderStatus)
                        .Select(g => new
                        {
                            status = g.Key,
                            count = g.Count()
                        })
                        .ToList();

                    return Json(statusData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}