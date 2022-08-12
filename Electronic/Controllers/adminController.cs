using Electronic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Electronic.Controllers
{
    public class adminController : Controller
    {
        mainEntities db = new mainEntities();
        // GET: admin

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(customer avm)
        {
            customer ad = db.customers.Where(x => x.username == avm.username && x.password == avm.password).SingleOrDefault();
            if (ad != null)
            {
                Session["username"] = ad.username.ToString();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.error = "Invalid username or password";
            }
            return View();
        }
        public ActionResult logout()
        {
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("login", "admin");

        }


        public ActionResult adminLogin(string name, string pass)
        {

            if (name == "admin" && pass == "admin")
            {
                Session["admin"] = name.ToString();
                Session["username"] = Session["admin"];
                return RedirectToAction("cat");
            }
            else
            {
                ViewBag.errormsg = "Invalid admin username or password";
                return View("Login");
            }

        }


        public ActionResult cat()
        {
            List<category> all_data = db.categories.ToList();
            return View(all_data);
        }

        public ActionResult saveData(category categories)
        {
            db.categories.Add(categories);
            db.SaveChanges();
            return RedirectToAction("cat");
        }

        public ActionResult deleteData(int id)
        {
            category category = db.categories.Find(id);
            db.categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("cat");
        }

        public ActionResult updateData(category category)
        {
            category value1 = db.categories.Find(category.id);
            value1.id = category.id;
            value1.catName = category.catName;
            value1.catDesc = category.catDesc;

            db.Entry(value1).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("cat");
        }

        public ActionResult product()
        {
            ViewBag.catID = new SelectList(db.categories, "id", "catName");
            List<product> products = db.products.ToList();
            return View(products);
        }

        [HttpPost]
        public ActionResult product(int catID)
        {
            ViewBag.catID = new SelectList(db.categories, "id", "catName");
            return View(db.products.Where(x => x.category.id.Equals(catID)).ToList());

        }
        public ActionResult save()
        {
            ViewBag.catID = new SelectList(db.categories, "id", "catName");
            return View();
        }

        [HttpPost]
        public ActionResult save(product products, HttpPostedFileBase productImage)
        {
            string path = Server.MapPath("~/uploads");
            string file_name = productImage.FileName;
            string new_path = path + "/" + file_name;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            productImage.SaveAs(new_path);

            products.productImage = "~/uploads/" + file_name;

            db.products.Add(products);
            db.SaveChanges();
            return RedirectToAction("product");
        }

        public ActionResult delete(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("product");
        }

        public ActionResult update(product product, HttpPostedFileBase productImage)
        {
            product value1 = db.products.Find(product.id);
            value1.id = product.id;
            value1.productName = product.productName;
            value1.categoryId = product.categoryId;
            value1.price = product.price;
            value1.productDesc = product.productDesc;
            string path = Server.MapPath("~/uploads");
            string file_name = productImage.FileName;
            string new_path = path + "/" + file_name;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            productImage.SaveAs(new_path);
            value1.productImage = "~/uploads/" + file_name;

            db.Entry(value1).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("product");
        }

        public ActionResult customerData(customer customer, HttpPostedFileBase profilePic)
        {
            string path = Server.MapPath("~/uploads");
            string file_name = profilePic.FileName;
            string new_path = path + "/" + file_name;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            profilePic.SaveAs(new_path);

            customer.profilePic = "~/uploads/" + file_name;
            db.customers.Add(customer);
            db.SaveChanges();
            return RedirectToAction("login");
        }

    }
}