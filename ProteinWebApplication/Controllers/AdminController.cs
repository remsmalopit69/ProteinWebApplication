using ProteinWebApplication.Models;
using ProteinWebApplication.Models.Context;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ProteinWebApplication.Controllers
{
    public class AdminController : Controller
    {
        // ==================== AUTHORIZATION CHECK ====================
        private bool IsAdmin()
        {
            return Session["UserRole"]?.ToString() == "admin";
        }

        private ActionResult CheckAdminAccess()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Shop");
            }
            return null;
        }

        public JsonResult LogoutAdmin()
        {
            Session.Clear();
            // AllowGet is required because Service.js uses $http.get()
            return Json(new { success = true, message = "Logged out successfully" }, JsonRequestBehavior.AllowGet);
        }

        // ==================== VIEWS ====================
        public ActionResult Dashboard()
        {
            var authCheck = CheckAdminAccess();
            if (authCheck != null) return authCheck;
            return View();
        }

        public ActionResult Categories()
        {
            var authCheck = CheckAdminAccess();
            if (authCheck != null) return authCheck;
            return View();
        }

        public ActionResult Products()
        {
            var authCheck = CheckAdminAccess();
            if (authCheck != null) return authCheck;
            return View();
        }

        public ActionResult Images()
        {
            var authCheck = CheckAdminAccess();
            if (authCheck != null) return authCheck;
            return View();
        }

        public ActionResult Orders()
        {
            var authCheck = CheckAdminAccess();
            if (authCheck != null) return authCheck;
            return View();
        }

        public ActionResult Reports()
        {
            var authCheck = CheckAdminAccess();
            if (authCheck != null) return authCheck;
            return View();
        }

        // ==================== CATEGORIES CRUD ====================
        public JsonResult GetCategories()
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

            try
            {
                using (var db = new ProteinContext())
                {
                    var images = db.tbl_images
                        .Where(x => x.isArchive == 0)
                        .OrderByDescending(x => x.createdAt)
                        .Select(i => new
                        {
                            i.imageID,
                            i.imageName,
                            i.imagePath,
                            i.imageType,
                            i.referenceID,
                            i.createdAt,
                            isAssigned = i.referenceID != null
                        })
                        .ToList();
                    return Json(images, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"An error occurred: {ex.Message}");
            }
        }


        // Get single image by type (for hero, banners, etc.)
        public JsonResult GetImageByType(string imageType)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

            try
            {
                using (var db = new ProteinContext())
                {
                    var image = db.tbl_images
                        .Where(x => x.isArchive == 0 && x.imageType == imageType)
                        .OrderByDescending(x => x.createdAt)
                        .Select(i => new { i.imageID, i.imagePath, i.imageName })
                        .FirstOrDefault();
                    return Json(image, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileBase imageFile, string imageType, int? referenceID)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
                            displayOrder = 0, // Not used anymore but keep for compatibility
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

        // Replace the ArchiveImage method in AdminController.cs with this fixed version:

        [HttpPost]
        public JsonResult ArchiveImage(int imageID)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
                        image.updatedAt = DateTime.Now;
                        db.SaveChanges();

                        var images = db.tbl_images
                            .Where(x => x.isArchive == 0)
                            .OrderByDescending(x => x.createdAt)
                            .Select(i => new
                            {
                                i.imageID,
                                i.imageName,
                                i.imagePath,
                                i.imageType,
                                i.referenceID,
                                i.createdAt,
                                isAssigned = i.referenceID != null
                            })
                            .ToList();

                        return Json(new { success = true, data = images, message = "Image archived successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "Image not found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SetAsActiveImage(int imageID, string imageType)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

            try
            {
                using (var db = new ProteinContext())
                {
                    // Archive all other images of this type without referenceID
                    var otherImages = db.tbl_images
                        .Where(x => x.imageType == imageType && x.referenceID == null && x.imageID != imageID)
                        .ToList();

                    foreach (var img in otherImages)
                    {
                        img.isArchive = 1;
                    }

                    // Make sure the selected image is active
                    var selectedImage = db.tbl_images.FirstOrDefault(x => x.imageID == imageID);
                    if (selectedImage != null)
                    {
                        selectedImage.isArchive = 0;
                        selectedImage.updatedAt = DateTime.Now;
                    }

                    db.SaveChanges();
                    return Json(new { success = true, message = "Image set as active" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Add this method to AdminController.cs after the GetImages method

        public JsonResult GetArchivedImages()
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

            try
            {
                using (var db = new ProteinContext())
                {
                    var images = db.tbl_images
                        .Where(x => x.isArchive == 1)
                        .OrderByDescending(x => x.updatedAt)
                        .Select(i => new
                        {
                            i.imageID,
                            i.imageName,
                            i.imagePath,
                            i.imageType,
                            i.referenceID,
                            i.createdAt,
                            i.updatedAt,
                            isAssigned = i.referenceID != null
                        })
                        .ToList();
                    return Json(images, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"An error occurred: {ex.Message}");
            }
        }

        // Add this method to restore archived images
        public JsonResult RestoreImage(int imageID)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

            try
            {
                using (var db = new ProteinContext())
                {
                    var image = db.tbl_images
                        .Where(x => x.imageID == imageID)
                        .FirstOrDefault();

                    if (image != null)
                    {
                        image.isArchive = 0;
                        image.updatedAt = DateTime.Now;
                        db.SaveChanges();

                        var archivedImages = db.tbl_images
                            .Where(x => x.isArchive == 1)
                            .OrderByDescending(x => x.updatedAt)
                            .Select(i => new
                            {
                                i.imageID,
                                i.imageName,
                                i.imagePath,
                                i.imageType,
                                i.referenceID,
                                i.createdAt,
                                i.updatedAt,
                                isAssigned = i.referenceID != null
                            })
                            .ToList();

                        return Json(new { success = true, data = archivedImages, message = "Image restored successfully" }, JsonRequestBehavior.AllowGet);
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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

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

        public JsonResult GetImagesByType(string imageType)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

            try
            {
                using (var db = new ProteinContext())
                {
                    var images = db.tbl_images
                        .Where(x => x.isArchive == 0 && x.imageType == imageType)
                        .OrderBy(x => x.displayOrder)
                        .ToList();
                    return Json(images, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AssignImageToProduct(int productID, int imageID)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

            try
            {
                using (var db = new ProteinContext())
                {
                    var image = db.tbl_images.FirstOrDefault(x => x.imageID == imageID);
                    if (image != null)
                    {
                        image.referenceID = productID;
                        image.imageType = "product";
                        db.SaveChanges();
                        return Json(new { success = true, message = "Image assigned successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "Image not found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AssignImageToCategory(int categoryID, int imageID)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

            try
            {
                using (var db = new ProteinContext())
                {
                    var image = db.tbl_images.FirstOrDefault(x => x.imageID == imageID);
                    if (image != null)
                    {
                        image.referenceID = categoryID;
                        image.imageType = "category";
                        db.SaveChanges();
                        return Json(new { success = true, message = "Image assigned successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = false, message = "Image not found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GenerateSalesReport()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            try
            {
                using (var db = new ProteinContext())
                {
                    var orders = db.tbl_orders
                        .Where(x => x.isArchive == 0)
                        .OrderByDescending(x => x.orderDate)
                        .ToList();

                    var doc = new Document(PageSize.A4, 25, 25, 30, 30);
                    var ms = new MemoryStream();
                    PdfWriter.GetInstance(doc, ms);
                    doc.Open();

                    // Header
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    var title = new Paragraph("Sales Report - ProteinPH", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    title.SpacingAfter = 20;
                    doc.Add(title);

                    // Date
                    var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    var datePara = new Paragraph($"Generated on: {DateTime.Now:MMMM dd, yyyy HH:mm}", dateFont);
                    datePara.Alignment = Element.ALIGN_RIGHT;
                    datePara.SpacingAfter = 20;
                    doc.Add(datePara);

                    // Summary
                    var totalRevenue = orders.Sum(x => x.totalAmount);
                    var totalOrders = orders.Count;
                    var avgOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

                    var summaryFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                    doc.Add(new Paragraph($"Total Orders: {totalOrders}", summaryFont));
                    doc.Add(new Paragraph($"Total Revenue: ₱{totalRevenue:N2}", summaryFont));
                    doc.Add(new Paragraph($"Average Order Value: ₱{avgOrderValue:N2}", summaryFont));
                    doc.Add(new Paragraph(" "));

                    // Orders Table
                    var table = new PdfPTable(5);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 1, 3, 2, 2, 2 });
                    table.SpacingBefore = 20;

                    // Table Header
                    var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                    var headerBg = new BaseColor(240, 240, 240);

                    AddCell(table, "Order ID", headerFont, headerBg);
                    AddCell(table, "Customer", headerFont, headerBg);
                    AddCell(table, "Date", headerFont, headerBg);
                    AddCell(table, "Status", headerFont, headerBg);
                    AddCell(table, "Amount", headerFont, headerBg);

                    // Table Data
                    var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                    foreach (var order in orders)
                    {
                        AddCell(table, order.orderID.ToString(), cellFont);
                        AddCell(table, order.customerName, cellFont);
                        AddCell(table, order.orderDate.ToString("MM/dd/yyyy"), cellFont);
                        AddCell(table, order.orderStatus, cellFont);
                        AddCell(table, $"₱{order.totalAmount:N2}", cellFont);
                    }

                    doc.Add(table);
                    doc.Close();

                    var pdfBytes = ms.ToArray();
                    return File(pdfBytes, "application/pdf", $"SalesReport_{DateTime.Now:yyyyMMdd}.pdf");
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Inventory Report
        public ActionResult GenerateInventoryReport()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            try
            {
                using (var db = new ProteinContext())
                {
                    var products = db.tbl_products
                        .Where(x => x.isArchive == 0)
                        .OrderBy(x => x.productName)
                        .ToList();

                    var categories = db.tbl_categories
                        .Where(x => x.isArchive == 0)
                        .ToDictionary(c => c.categoryID, c => c.categoryName);

                    var doc = new Document(PageSize.A4, 25, 25, 30, 30);
                    var ms = new MemoryStream();
                    PdfWriter.GetInstance(doc, ms);
                    doc.Open();

                    // Header
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    var title = new Paragraph("Inventory Report - ProteinPH", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    title.SpacingAfter = 20;
                    doc.Add(title);

                    // Date
                    var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    var datePara = new Paragraph($"Generated on: {DateTime.Now:MMMM dd, yyyy HH:mm}", dateFont);
                    datePara.Alignment = Element.ALIGN_RIGHT;
                    datePara.SpacingAfter = 20;
                    doc.Add(datePara);

                    // Summary
                    var totalProducts = products.Count;
                    var totalStock = products.Sum(x => x.stockQuantity);
                    var outOfStock = products.Count(x => x.stockQuantity == 0);
                    var lowStock = products.Count(x => x.stockQuantity > 0 && x.stockQuantity < 10);

                    var summaryFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                    doc.Add(new Paragraph($"Total Products: {totalProducts}", summaryFont));
                    doc.Add(new Paragraph($"Total Stock Units: {totalStock}", summaryFont));
                    doc.Add(new Paragraph($"Out of Stock: {outOfStock} products", summaryFont));
                    doc.Add(new Paragraph($"Low Stock (<10): {lowStock} products", summaryFont));
                    doc.Add(new Paragraph(" "));

                    // Products Table
                    var table = new PdfPTable(5);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 1, 4, 2, 2, 2 });
                    table.SpacingBefore = 20;

                    // Table Header
                    var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                    var headerBg = new BaseColor(240, 240, 240);

                    AddCell(table, "ID", headerFont, headerBg);
                    AddCell(table, "Product Name", headerFont, headerBg);
                    AddCell(table, "Category", headerFont, headerBg);
                    AddCell(table, "Stock", headerFont, headerBg);
                    AddCell(table, "Price", headerFont, headerBg);

                    // Table Data
                    var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                    foreach (var product in products)
                    {
                        var categoryName = categories.ContainsKey(product.categoryID) ? categories[product.categoryID] : "N/A";
                        var stockColor = product.stockQuantity == 0 ? BaseColor.RED :
                                        product.stockQuantity < 10 ? BaseColor.ORANGE : BaseColor.BLACK;

                        AddCell(table, product.productID.ToString(), cellFont);
                        AddCell(table, product.productName, cellFont);
                        AddCell(table, categoryName, cellFont);
                        AddCell(table, product.stockQuantity.ToString(), cellFont, null, stockColor);
                        AddCell(table, $"₱{product.price:N2}", cellFont);
                    }

                    doc.Add(table);
                    doc.Close();

                    var pdfBytes = ms.ToArray();
                    return File(pdfBytes, "application/pdf", $"InventoryReport_{DateTime.Now:yyyyMMdd}.pdf");
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Helper method for adding cells
        private void AddCell(PdfPTable table, string text, Font font, BaseColor bgColor = null, BaseColor textColor = null)
        {
            var cell = new PdfPCell(new Phrase(text, font));
            if (bgColor != null) cell.BackgroundColor = bgColor;
            if (textColor != null) cell.Phrase.Font.Color = textColor;
            cell.Padding = 8;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);
        }

    }
}