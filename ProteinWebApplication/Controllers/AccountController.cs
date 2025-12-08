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
                using (var db = new ProteinContext())
                {
                    // Check if username already exists
                    var existingUsername = db.tbl_users
                        .Where(x => x.username == user.username && x.isArchive == 0)
                        .FirstOrDefault();

                    if (existingUsername != null)
                    {
                        return Json(new { success = false, message = "Username already exists" }, JsonRequestBehavior.AllowGet);
                    }

                    // Check if email already exists
                    var existingEmail = db.tbl_users
                        .Where(x => x.email == user.email && x.isArchive == 0)
                        .FirstOrDefault();

                    if (existingEmail != null)
                    {
                        return Json(new { success = false, message = "Email already exists" }, JsonRequestBehavior.AllowGet);
                    }

                    // Create new user
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
                        return Json(new { success = true, message = "Login successful" }, JsonRequestBehavior.AllowGet);
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
    }
}