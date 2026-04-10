using Microsoft.AspNetCore.Mvc;
using StoreApp.Data;
using StoreApp.Models;

namespace StoreApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

		// عرض جميع المنتجات + البحث
		public IActionResult Index(string search)
		{
			var products = _context.Products.AsQueryable();

			if (!string.IsNullOrEmpty(search))
			{
				products = products.Where(p =>
					p.Name.Contains(search) ||
					p.Quantity.ToString().Contains(search) ||
					p.UnitPrice.ToString().Contains(search) ||
					p.initialPrice.ToString().Contains(search));
			}

			return View(products.ToList());
		}
		// صفحة إضافة منتج
		public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(product);
        }

        // تعديل
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound();

            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Content(string.Join(" | ", errors));
            }

            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);

            if (existingProduct == null)
                return Content("المنتج غير موجود");

            existingProduct.Name = product.Name;
            existingProduct.Quantity = product.Quantity;
            existingProduct.initialPrice = product.initialPrice;
            existingProduct.UnitPrice = product.UnitPrice;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // حذف
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

		// عرض تفاصيل المنتج
		public IActionResult Details(int id)
		{
			var product = _context.Products.Find(id);

			if (product == null)
				return NotFound();

			return View(product);
		}
        
    }
}