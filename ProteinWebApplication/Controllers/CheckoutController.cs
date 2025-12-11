using ProteinWebApplication.Models;
using ProteinWebApplication.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProteinWebApplication.Controllers
{
    public class CheckoutController : Controller
    {
        // ==================== AUTHORIZATION CHECK ====================
        private bool IsUserLoggedIn()
        {
            return Session["UserID"] != null;
        }

        private ActionResult CheckUserAccess()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
            return null;
        }

        // ==================== VIEWS ====================
        public ActionResult Index()
        {
            var authCheck = CheckUserAccess();
            if (authCheck != null) return authCheck;
            return View();
        }

        // FIXED: Make orderID nullable with default value
        public ActionResult Success(int? orderID = null)
        {
            var authCheck = CheckUserAccess();
            if (authCheck != null) return authCheck;

            // If no orderID provided, redirect to shop
            if (!orderID.HasValue)
            {
                return RedirectToAction("Index", "Shop");
            }

            ViewBag.OrderID = orderID.Value;
            return View();
        }

        // ==================== CHECKOUT API ====================
        [HttpPost]
        public JsonResult ProcessCheckout(CheckoutModel checkoutData)
        {
            if (!IsUserLoggedIn())
            {
                return Json(new { success = false, message = "Please login to continue" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                using (var db = new ProteinContext())
                {
                    // Get user ID from session
                    int userID = Convert.ToInt32(Session["UserID"]);

                    // Create new order
                    var order = new tblOrdersModel
                    {
                        customerName = checkoutData.customerName,
                        customerEmail = checkoutData.customerEmail,
                        customerPhone = checkoutData.customerPhone,
                        shippingAddress = checkoutData.shippingAddress,
                        totalAmount = checkoutData.totalAmount,
                        orderStatus = "pending",
                        orderDate = DateTime.Now,
                        createdAt = DateTime.Now,
                        updatedAt = DateTime.Now,
                        isArchive = 0
                    };

                    db.tbl_orders.Add(order);
                    db.SaveChanges();

                    // Save order items
                    if (checkoutData.orderItems != null && checkoutData.orderItems.Count > 0)
                    {
                        foreach (var item in checkoutData.orderItems)
                        {
                            var orderItem = new tblOrderItemsModel
                            {
                                orderID = order.orderID,
                                productID = item.productID,
                                quantity = item.quantity,
                                unitPrice = item.unitPrice,
                                subtotal = item.quantity * item.unitPrice,
                                createdAt = DateTime.Now,
                                isArchive = 0
                            };

                            db.tbl_order_items.Add(orderItem);

                            // Update product stock
                            var product = db.tbl_products.FirstOrDefault(p => p.productID == item.productID);
                            if (product != null)
                            {
                                product.stockQuantity -= item.quantity;
                                product.updatedAt = DateTime.Now;
                            }
                        }

                        db.SaveChanges();
                    }

                    return Json(new
                    {
                        success = true,
                        orderID = order.orderID,
                        message = "Order placed successfully!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        // Get user's orders
        public JsonResult GetMyOrders()
        {
            if (!IsUserLoggedIn())
            {
                return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                using (var db = new ProteinContext())
                {
                    int userID = Convert.ToInt32(Session["UserID"]);

                    // For now, get all orders by customer email (since we don't have userID in orders table)
                    var userEmail = Session["UserEmail"]?.ToString();

                    var orders = db.tbl_orders
                        .Where(x => x.customerEmail == userEmail && x.isArchive == 0)
                        .OrderByDescending(x => x.orderDate)
                        .Select(o => new
                        {
                            o.orderID,
                            o.customerName,
                            o.totalAmount,
                            o.orderStatus,
                            o.orderDate,
                            o.shippingAddress
                        })
                        .ToList();

                    return Json(orders, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Get specific order details
        public JsonResult GetOrderDetails(int orderID)
        {
            if (!IsUserLoggedIn())
            {
                return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                using (var db = new ProteinContext())
                {
                    var order = db.tbl_orders
                        .Where(x => x.orderID == orderID && x.isArchive == 0)
                        .Select(o => new
                        {
                            o.orderID,
                            o.customerName,
                            o.customerEmail,
                            o.customerPhone,
                            o.shippingAddress,
                            o.totalAmount,
                            o.orderStatus,
                            o.orderDate,
                            items = db.tbl_order_items
                                .Where(i => i.orderID == orderID && i.isArchive == 0)
                                .Select(i => new
                                {
                                    i.orderItemID,
                                    i.productID,
                                    productName = db.tbl_products
                                        .Where(p => p.productID == i.productID)
                                        .Select(p => p.productName)
                                        .FirstOrDefault(),
                                    i.quantity,
                                    i.unitPrice,
                                    i.subtotal
                                })
                                .ToList()
                        })
                        .FirstOrDefault();

                    if (order == null)
                    {
                        return Json(new { success = false, message = "Order not found" }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(order, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }

    // ==================== MODELS ====================
    public class CheckoutModel
    {
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public string customerPhone { get; set; }
        public string shippingAddress { get; set; }
        public decimal totalAmount { get; set; }
        public List<OrderItemModel> orderItems { get; set; }
    }

    public class OrderItemModel
    {
        public int productID { get; set; }
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
    }
}