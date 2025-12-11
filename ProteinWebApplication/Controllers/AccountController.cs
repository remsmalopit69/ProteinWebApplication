using ProteinWebApplication.Models;
using ProteinWebApplication.Models.Context;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProteinWebApplication.Controllers
{
    public class AccountController : Controller
    {
        // ==================== VIEWS ====================
        public ActionResult Login()
        {
            // If user is already logged in, redirect to shop
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Shop");
            }
            return View();
        }

        public ActionResult Register()
        {
            // If user is already logged in, redirect to shop
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Shop");
            }
            return View();
        }

        // ==================== USER AUTHENTICATION ====================
        public JsonResult RegisterUser(tblUsersModel user)
        {
            try
            {
                // Server-side validation
                if (string.IsNullOrWhiteSpace(user.fullName) || user.fullName.Length < 2)
                {
                    return Json(new { success = false, message = "Full name must be at least 2 characters" }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace(user.username) || user.username.Length < 3)
                {
                    return Json(new { success = false, message = "Username must be at least 3 characters" }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace(user.email) || !IsValidEmail(user.email))
                {
                    return Json(new { success = false, message = "Invalid email address" }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace(user.phoneNumber) || !IsValidPhoneNumber(user.phoneNumber))
                {
                    return Json(new { success = false, message = "Invalid phone number format" }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace(user.address) || user.address.Length < 10)
                {
                    return Json(new { success = false, message = "Address must be at least 10 characters" }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace(user.password) || user.password.Length < 6)
                {
                    return Json(new { success = false, message = "Password must be at least 6 characters" }, JsonRequestBehavior.AllowGet);
                }

                using (var db = new ProteinContext())
                {
                    // Check if username already exists
                    var existingUsername = db.tbl_users
                        .Where(x => x.username.ToLower() == user.username.ToLower() && x.isArchive == 0)
                        .FirstOrDefault();

                    if (existingUsername != null)
                    {
                        return Json(new { success = false, message = "Username already exists" }, JsonRequestBehavior.AllowGet);
                    }

                    // Check if email already exists
                    var existingEmail = db.tbl_users
                        .Where(x => x.email.ToLower() == user.email.ToLower() && x.isArchive == 0)
                        .FirstOrDefault();

                    if (existingEmail != null)
                    {
                        return Json(new { success = false, message = "Email already exists" }, JsonRequestBehavior.AllowGet);
                    }

                    // Sanitize inputs
                    user.fullName = user.fullName.Trim();
                    user.username = user.username.Trim().ToLower();
                    user.email = user.email.Trim().ToLower();
                    user.phoneNumber = user.phoneNumber.Trim();
                    user.address = user.address.Trim();
                    user.role = "customer";
                    user.createdAt = DateTime.Now;
                    user.updatedAt = DateTime.Now;
                    user.isArchive = 0;

                    db.tbl_users.Add(user);
                    db.SaveChanges();

                    return Json(new { success = true, message = "Registration successful! Please login." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }
        // Helper methods
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Remove common formatting characters
            string cleanNumber = phoneNumber.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

            // Check if it contains only digits and has reasonable length (10-15 digits)
            return System.Text.RegularExpressions.Regex.IsMatch(cleanNumber, @"^\d{10,15}$");
        }
        public JsonResult LoginUser(string username, string password)
        {
            try
            {
                using (var db = new ProteinContext())
                {
                    // Check if user exists
                    var user = db.tbl_users
                        .Where(x => x.username == username && x.password == password && x.isArchive == 0)
                        .FirstOrDefault();

                    if (user != null)
                    {
                        Session["UserID"] = user.userID;
                        Session["UserName"] = user.fullName;
                        Session["UserEmail"] = user.email;
                        Session["UserRole"] = user.role; // Optional: Save role to session if needed later
                        return Json(new { success = true, message = "Login successful", role = user.role }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Invalid username or password" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LogoutUser()
        {
            Session.Clear();
            return RedirectToAction("Index", "Shop");
        }

        public JsonResult GetCurrentUser()
        {
            try
            {
                if (Session["UserID"] != null)
                {
                    return Json(new
                    {
                        success = true,
                        isLoggedIn = true,
                        userID = Session["UserID"],
                        userName = Session["UserName"],
                        userEmail = Session["UserEmail"]
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        success = true,
                        isLoggedIn = false
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        //    // Process checkout and create order
        //    [HttpPost]
        //    public JsonResult ProcessCheckout(CheckoutModel checkoutData)
        //    {
        //        try
        //        {
        //            if (Session["UserID"] == null)
        //            {
        //                return Json(new { success = false, message = "Please login to checkout" }, JsonRequestBehavior.AllowGet);
        //            }

        //            using (var db = new ProteinContext())
        //            {
        //                // Create new order
        //                var order = new tblOrdersModel
        //                {
        //                    customerName = checkoutData.customerName,
        //                    customerEmail = checkoutData.customerEmail,
        //                    customerPhone = checkoutData.customerPhone,
        //                    shippingAddress = checkoutData.shippingAddress,
        //                    totalAmount = checkoutData.totalAmount,
        //                    orderStatus = "pending",
        //                    orderDate = DateTime.Now,
        //                    createdAt = DateTime.Now,
        //                    updatedAt = DateTime.Now,
        //                    isArchive = 0
        //                };

        //                db.tbl_orders.Add(order);
        //                db.SaveChanges();

        //                // Save order items
        //                foreach (var item in checkoutData.items)
        //                {
        //                    var orderItem = new tblOrderItemsModel
        //                    {
        //                        orderID = order.orderID,
        //                        productID = item.productID,
        //                        quantity = item.quantity,
        //                        unitPrice = item.unitPrice,
        //                        subtotal = item.quantity * item.unitPrice,
        //                        createdAt = DateTime.Now,
        //                        isArchive = 0
        //                    };

        //                    db.tbl_order_items.Add(orderItem);

        //                    // Update product stock
        //                    var product = db.tbl_products.FirstOrDefault(p => p.productID == item.productID);
        //                    if (product != null)
        //                    {
        //                        product.stockQuantity -= item.quantity;
        //                        product.updatedAt = DateTime.Now;
        //                    }
        //                }

        //                db.SaveChanges();

        //                return Json(new
        //                {
        //                    success = true,
        //                    message = "Order placed successfully!",
        //                    orderID = order.orderID
        //                }, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    // Get user's orders
        //    public JsonResult GetMyOrders()
        //    {
        //        try
        //        {
        //            if (Session["UserID"] == null)
        //            {
        //                return Json(new { success = false, message = "Not logged in" }, JsonRequestBehavior.AllowGet);
        //            }

        //            var userEmail = Session["UserEmail"]?.ToString();

        //            using (var db = new ProteinContext())
        //            {
        //                var orders = db.tbl_orders
        //                    .Where(x => x.customerEmail == userEmail && x.isArchive == 0)
        //                    .OrderByDescending(x => x.orderDate)
        //                    .Select(o => new
        //                    {
        //                        o.orderID,
        //                        o.customerName,
        //                        o.totalAmount,
        //                        o.orderStatus,
        //                        o.orderDate,
        //                        itemCount = db.tbl_order_items.Count(i => i.orderID == o.orderID)
        //                    })
        //                    .ToList();

        //                return Json(orders, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    // Get order details
        //    public JsonResult GetOrderDetails(int orderID)
        //    {
        //        try
        //        {
        //            using (var db = new ProteinContext())
        //            {
        //                var order = db.tbl_orders
        //                    .Where(x => x.orderID == orderID)
        //                    .Select(o => new
        //                    {
        //                        o.orderID,
        //                        o.customerName,
        //                        o.customerEmail,
        //                        o.customerPhone,
        //                        o.shippingAddress,
        //                        o.totalAmount,
        //                        o.orderStatus,
        //                        o.orderDate,
        //                        items = db.tbl_order_items
        //                            .Where(i => i.orderID == orderID)
        //                            .Select(i => new
        //                            {
        //                                i.orderItemID,
        //                                i.productID,
        //                                productName = db.tbl_products
        //                                    .Where(p => p.productID == i.productID)
        //                                    .Select(p => p.productName)
        //                                    .FirstOrDefault(),
        //                                i.quantity,
        //                                i.unitPrice,
        //                                i.subtotal
        //                            })
        //                            .ToList()
        //                    })
        //                    .FirstOrDefault();

        //                return Json(order, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}
        //// Model for checkout data
        //public class CheckoutModel
        //{
        //    public string customerName { get; set; }
        //    public string customerEmail { get; set; }
        //    public string customerPhone { get; set; }
        //    public string shippingAddress { get; set; }
        //    public decimal totalAmount { get; set; }
        //    public CheckoutItemModel[] items { get; set; }
        //}

        //public class CheckoutItemModel
        //{
        //    public int productID { get; set; }
        //    public int quantity { get; set; }
        //    public decimal unitPrice { get; set; }
    }
}
