using Electronic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Electronic.Controllers
{
    public class homeController : Controller
    {
        mainEntities db = new mainEntities();
        // GET: home
        public ActionResult Index()
        {
            var all_data = db.categories.ToList();
            return View(all_data);
        }
        public ActionResult category(int? i)
        {
            //if (Session["username"] == null && Session["admin"] == null)
            //{
            //    return RedirectToAction("login", "admin");
            //}
            List<category> all_data = db.categories.ToList();
            return View(all_data);
        }

        public ActionResult product()
        {
            List<product> all_data = db.products.ToList();
            return View(all_data);
        }

        [HttpPost]
        public ActionResult product( string option, string search)
        {
            var all_data = db.products.ToList();
            if (option == "name")
            {
                all_data = db.products.Where(x => x.productName.Contains(search)).ToList();
                return View(all_data);
            }
            else if (option == "cat")
            {
                all_data = db.products.Where(x => x.category.catName.Contains(search)).ToList();
                return View(all_data);
            }
            else
            {
                return View(all_data);
            }

        }

        public ActionResult Contact()
        {
            if (Session["username"] == null && Session["admin"] == null)
            {
                return RedirectToAction("login", "admin");
            }
            return View();
        }
        public ActionResult cart()
        {
            List<cart> all_data = db.carts.ToList();
            return View(all_data);
        }

        public ActionResult myCart(int id)
        {
            product value = db.products.Find(id);
            cart mycart = new cart();
            //mycart.userName = (string)Session["username"];
            mycart.productName = value.productName;
            mycart.price = value.price;
            mycart.productImage = value.productImage;
            db.carts.Add(mycart);
            db.SaveChanges();
            return RedirectToAction("product");
        }

        public ActionResult MakePayment()
        {
            var value = db.carts.ToList();
            db.carts.RemoveRange(value);
            db.SaveChanges();
            return RedirectToAction("cart");
        }

        public ActionResult deleteData(int id)
        {
            cart cart = db.carts.Find(id);
            db.carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("cart");
        }

    }
}