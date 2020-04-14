using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Emarket.Models;
using System.IO;


namespace Emarket.Controllers
{
    public class ProductsController : Controller
    {
        private EmarketDBEntities db = new EmarketDBEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Layout()
        {
            var record = db.Products.ToList();
            return View(record);
        }
        public ActionResult Search(string key)
        {
            //var categoryID = (from p in db.Categories
            //                  where p.name == key
            //                  select new Category { id = p.id });
            var categoryID = (from p in db.Categories
                              where p.name == key
                              select p.id).FirstOrDefault();
           // int parsed = int.Parse(categoryID);
            var listOfProducts = db.Products.Where(x => x.category_id== categoryID).ToList();
            /* */
            //  SqlCommand cmd = new SqlCommand(categoryID);  
            //  int parsed = Convert.ToInt32(categoryID);
            //  var listOfProducts = db.Products.Where(x => x.id == parsed).ToList();
            return View(listOfProducts);
        }
        public ActionResult MoreInfo(int id)
        {
            Product P = new Product();

            using (db)
            {
				P = db.Products.Include(x => x.Category).SingleOrDefault(p => p.id == id);
            }
            return View(P);
        }
		[HttpGet]
		public ActionResult Add()
		{
			ViewBag.category_id = new SelectList(db.Categories, "id", "name");
			return View();
		}
		[HttpPost]
		public ActionResult Add(Product product)
		{
			if (product.ImageFile == null)
			{
				ViewBag.error = "This is Required";
			}
			else
			{
				string filename = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
				string Extention = Path.GetExtension(product.ImageFile.FileName);
				filename += Extention;
				product.image = "~/photos/" + filename;
				filename = Path.Combine(Server.MapPath("~/photos/"), filename);
				if (Extention.ToLower() == ".jpg" || Extention.ToLower() == ".png" || Extention.ToLower() == ".jpeg")
				{
					if (ModelState.IsValid)
					{
						product.ImageFile.SaveAs(filename);
						db.Products.Add(product);
						var item2 = db.Categories.Find(product.category_id);
						item2.number_of_products++;
						db.Entry(item2).State = EntityState.Modified;
						db.SaveChanges();
						return RedirectToAction("Layout");
					}
				}
				else
				{
					ViewBag.msg = "Invalid File Type";
				}
			}
			ModelState.Clear();
			ViewBag.category_id = new SelectList(db.Categories, "id", "name", product.category_id);
			return View();
		}
		public ActionResult Delete(int id)
		{
			var item = db.Products.Where(x => x.id == id).First();
			var item2 = db.Categories.Find(item.category_id);
			item2.number_of_products--;
			db.Entry(item2).State = EntityState.Modified;
			db.Products.Remove(item);
			db.SaveChanges();
			return RedirectToAction("Layout");
		}
		//public ActionResult Search(string key)
		//{

		//    IEnumerable<Category> rec;
		//    foreach (var item in db.Categories)
		//    {
		//        if (item.name == key)
		//        {
		//             rec = from id in db.Categories
		//                      select id;
		//        }
		//    }
		//    IEnumerable<Product> se = from p in db.Products
		//             where p.category_id == rec
		//             select p;


		//    return View();
		// }
		//    //}
		//[ActionName("Layout")]
		//public ActionResult Search(string key)
		//{
		//    //IEnumerable<Product> selection = from p in db.Products
		//    //                                 where p.Category.name == key
		//    //                                 select p;
		//    //IEnumerable<Category>
		//    var selection = from p in db.Categories
		//                                      where p.name==key
		//                                      select new {p.id};
		//    var bsmallah = from q in db.Products
		//                                    where selection.Equals(q.id)
		//                                    select q;
		//    return View(bsmallah);
		//}


	}
}
