﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Emarket.Models;


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

            EmarketDBEntities emarketDB = new EmarketDBEntities();
            var categorylist = emarketDB.Categories.ToList();
            SelectList list = new SelectList(categorylist, "id","name");

            ViewBag.CategoryList = list;


            var record = db.Products.ToList();
            return View(record);
        }
        public ActionResult Search(string key)
        {
            EmarketDBEntities emarketDB = new EmarketDBEntities();
            var categorylist = emarketDB.Categories.ToList();
            SelectList list = new SelectList(categorylist, "id", "name");
            ViewBag.CategoryList = list;
            //
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
