using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsCategories.Models;

namespace ProductsCategories.Controllers {
    public class HomeController : Controller {
        private MyContext _context { get; set; }

        public HomeController (MyContext context) {
            _context = context;
        }

        [HttpGet ("")]
        public IActionResult Index () {
            ViewBag.Product = _context.Products.ToList ();
            return View ();
        }

        [HttpGet ("Category")]
        public IActionResult Category () {
            ViewBag.Categories = _context.Categories.ToList ();
            return View ();
        }

        [HttpPost ("AddProduct")]
        public IActionResult AddProduct (Product prod) {
            if (ModelState.IsValid) {
                _context.Products.Add (prod);
                _context.SaveChanges ();
                return Redirect ("/");
            }
            return View ("Index");
        }

        [HttpPost ("AddCategory")]
        public IActionResult AddCategory (Category cat) {
            if (ModelState.IsValid) {
                _context.Categories.Add (cat);
                _context.SaveChanges ();
                return Redirect ("Category");
            }
            return View ("Category");
        }

        [HttpGet ("Category/{cat}")]
        public IActionResult Categories (int cat) {
            ViewBag.Categories = _context.Categories.FirstOrDefault (c => c.CatId == cat);
            // ViewBag.ProductsIn = _context.Categories.Include (l => l.CategorySelections).ThenInclude (c => c.ColProd).FirstOrDefault (c => c.CatId == cat);
            ViewBag.NotinCat = _context.Products.Include (z => z.ProdCollections).Where (c => c.ProdCollections.All (i => i.CatId != cat)).ToList ();
            Category topass = _context.Categories.Include (c => c.CategorySelections).ThenInclude (p => p.ColProd).FirstOrDefault (c => c.CatId == cat);
            return View (topass);
        }

        [HttpGet ("Product/{prod}")]
        public IActionResult Products (int prod) {
            ViewBag.NotInCol = _context.Categories.Include (c => c.CategorySelections).Where (c => c.CategorySelections.All (i => i.ProdId != prod)).ToList ();
            ViewBag.Products = _context.Products.FirstOrDefault (p => p.ProdId == prod);
            Product topass = _context.Products.Include (p => p.ProdCollections).ThenInclude (c => c.ColCat).FirstOrDefault (p => p.ProdId == prod);
            return View (topass);
        }

        [HttpPost ("AddProdCol/{prodid}")]
        public IActionResult AddProdCollection (int prodid, int catid) {
            Collection coll = new Collection ();
            coll.ProdId = prodid;
            coll.CatId = catid;
            _context.Collections.Add (coll);
            _context.SaveChanges ();
            return Redirect ($"/Product/{prodid}");
        }

        [HttpPost ("/AddCatCol/{catid}")]
        public IActionResult AddCatCollection (int catid, int prodid) {
            Collection coll = new Collection ();
            coll.ProdId = prodid;
            coll.CatId = catid;
            _context.Collections.Add (coll);
            _context.SaveChanges ();
            return Redirect ($"/Category/{catid}");
        }

        [HttpGet ("removeP/collection/{collid}")]
        public IActionResult PRemoveCollection (int collid) {
            Collection toremove = _context.Collections.FirstOrDefault (c => c.ColId == collid);
            _context.Collections.Remove (toremove);
            _context.SaveChanges ();
            return Redirect ($"/Product/{toremove.ProdId}");
        }

        [HttpGet ("removeC/collection/{collid}")]
        public IActionResult CRemoveCollection (int collid) {
            Collection toremove = _context.Collections.FirstOrDefault (c => c.ColId == collid);
            _context.Collections.Remove (toremove);
            _context.SaveChanges ();
            return Redirect ($"/Category/{toremove.CatId}");
        }

        [HttpGet ("category/destroy/{cat}")]
        public IActionResult DestroyCat (int cat) {
            Category toDestroy = _context.Categories.FirstOrDefault (x => x.CatId == cat);
            _context.Categories.Remove (toDestroy);
            _context.SaveChanges ();
            return RedirectToAction ("Category");
        }

        [HttpGet ("product/destroy/{prod}")]
        public IActionResult DestroyProd (int prod) {
            Product toDestroy = _context.Products.FirstOrDefault (x => x.ProdId == prod);
            _context.Products.Remove (toDestroy);
            _context.SaveChanges ();
            return RedirectToAction ("Index");
        }
    }
}