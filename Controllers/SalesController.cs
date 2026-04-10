using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data;
using StoreApp.Models;
using System.Linq;

namespace StoreApp.Controllers
{
    public class SalesController : Controller
    {
        private readonly AppDbContext _context;
        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Sale sale)
        {
            foreach (var item in sale.Items)
            {
                var product = _context.Products.Find(item.ProductId);

                if (product == null)
                    return NotFound();

                //  تحقق أولاً إذا كانت الكمية صفر
                if (product.Quantity == 0)
                {
                    ModelState.AddModelError("", $"المنتج '{product.Name}' غير متوفر بالمخزون حالياً (الكمية = 0)");
                    ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
                    return View(sale);
                }

                //  تحقق من الكمية المطلوبة أكبر من المخزون
                if (product.Quantity < item.QuantitySold)
                {
                    ModelState.AddModelError("", $"الكمية المطلوبة للمنتج '{product.Name}' أكبر من الكمية المتوفرة بالمخزون ({product.Quantity})");
                    ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
                    return View(sale);
                }

                item.InitialPrice = product.initialPrice;
                item.PurchaseCurrency = product.Currency;

                item.SaleCurrency = sale.SaleCurrency;
                item.ExchangeRate = sale.ExchangeRate;

                product.Quantity -= item.QuantitySold;
            }

            _context.Sales.Add(sale);
            _context.SaveChanges();

            //  ربط بالدين
            if (sale.IsDebt)
            {
                var debt = _context.CustomerDebts
                    .FirstOrDefault(d => d.CustomerName == sale.CustomerName);

                if (debt == null)
                {
                    debt = new CustomerDebt
                    {
                        CustomerName = sale.CustomerName
                    };

                    _context.CustomerDebts.Add(debt);
                    _context.SaveChanges();
                }

                sale.CustomerDebtId = debt.Id;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Index()
		{
            var sales = _context.Sales
    .Include(s => s.Items)
    .ThenInclude(i => i.Product)
    .Include(s => s.CustomerDebt)
    .ThenInclude(d => d.Payments)
    .OrderByDescending(s => s.SaleDate)
    .ToList();

            var today = DateTime.Today;

			var items = sales.SelectMany(s => s.Items);

			// أرباح اليوم
			ViewBag.DailyProfitUSD = items
				.Where(i => i.Sale.SaleDate.Date == today && i.SaleCurrency == "USD")
				.Sum(i => i.Profit);

			ViewBag.DailyProfitSYP = items
				.Where(i => i.Sale.SaleDate.Date == today && i.SaleCurrency == "SYP")
				.Sum(i => i.Profit);

			// مبيعات اليوم
			ViewBag.DailySalesUSD = items
				.Where(i => i.Sale.SaleDate.Date == today && i.SaleCurrency == "USD")
				.Sum(i => i.SellingPrice * i.QuantitySold);

			ViewBag.DailySalesSYP = items
				.Where(i => i.Sale.SaleDate.Date == today && i.SaleCurrency == "SYP")
				.Sum(i => i.SellingPrice * i.QuantitySold);

			// مبيعات الشهر
			ViewBag.MonthlySalesUSD = items
				.Where(i =>
					i.Sale.SaleDate.Month == today.Month &&
					i.Sale.SaleDate.Year == today.Year &&
					i.SaleCurrency == "USD")
				.Sum(i => i.SellingPrice * i.QuantitySold);

			ViewBag.MonthlySalesSYP = items
				.Where(i =>
					i.Sale.SaleDate.Month == today.Month &&
					i.Sale.SaleDate.Year == today.Year &&
					i.SaleCurrency == "SYP")
				.Sum(i => i.SellingPrice * i.QuantitySold);

			// الربح الشهري
			ViewBag.MonthlyProfitUSD = items
				.Where(i =>
					i.Sale.SaleDate.Month == today.Month &&
					i.Sale.SaleDate.Year == today.Year &&
					i.SaleCurrency == "USD")
				.Sum(i => i.Profit);

			ViewBag.MonthlyProfitSYP = items
				.Where(i =>
					i.Sale.SaleDate.Month == today.Month &&
					i.Sale.SaleDate.Year == today.Year &&
					i.SaleCurrency == "SYP")
				.Sum(i => i.Profit);

			return View(sales);
		}



	
		public IActionResult Edit(int id)
		{
			var sale = _context.Sales
				.Include(s => s.Items)
				.FirstOrDefault(s => s.Id == id);

			if (sale == null)
				return NotFound();

			ViewBag.Products = new SelectList(_context.Products, "Id", "Name");

			return View(sale);
		}

        [HttpPost]
        public IActionResult Edit(Sale sale)
        {
            var existingSale = _context.Sales
                .Include(s => s.Items)
                .FirstOrDefault(s => s.Id == sale.Id);

            if (existingSale == null)
                return NotFound();

            // إعادة الكميات القديمة للمخزون
            foreach (var item in existingSale.Items)
            {
                var product = _context.Products.Find(item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.QuantitySold;
                }
            }

            // حذف العناصر القديمة
            _context.SaleItem.RemoveRange(existingSale.Items);

            // إضافة العناصر الجديدة مع التحقق
            foreach (var item in sale.Items)
            {
                var product = _context.Products.Find(item.ProductId);

                if (product == null)
                    return NotFound();

                // تحقق إذا الكمية = 0
                if (product.Quantity == 0)
                {
                    ModelState.AddModelError("", $"المنتج '{product.Name}' غير متوفر بالمخزون حالياً (الكمية = 0)");
                    ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
                    return View(sale);
                }

                // تحقق إذا الكمية المطلوبة أكبر من المخزون
                if (product.Quantity < item.QuantitySold)
                {
                    ModelState.AddModelError("", $"الكمية المطلوبة للمنتج '{product.Name}' أكبر من الكمية المتوفرة بالمخزون ({product.Quantity})");
                    ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
                    return View(sale);
                }

                item.InitialPrice = product.initialPrice;
                item.PurchaseCurrency = product.Currency;
                item.SaleCurrency = sale.SaleCurrency;
                item.ExchangeRate = sale.ExchangeRate;

                product.Quantity -= item.QuantitySold;
            }

            existingSale.CustomerName = sale.CustomerName;
            existingSale.SaleCurrency = sale.SaleCurrency;
            existingSale.ExchangeRate = sale.ExchangeRate;
            existingSale.Items = sale.Items;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
		{
			var sale = _context.Sales
				.Include(s => s.Items)
				.ThenInclude(i => i.Product)
				.FirstOrDefault(s => s.Id == id);

			if (sale == null)
				return NotFound();

			return View(sale);
		}

		
		[HttpPost, ActionName("Delete")]
		public IActionResult DeleteConfirmed(int id)
		{
			var sale = _context.Sales
				.Include(s => s.Items)
				.FirstOrDefault(s => s.Id == id);

			if (sale == null)
				return NotFound();

			// إعادة الكميات للمخزون
			foreach (var item in sale.Items)
			{
				var product = _context.Products.Find(item.ProductId);

				if (product != null)
				{
					product.Quantity += item.QuantitySold;
				}
			}

			_context.Sales.Remove(sale);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}

	}
}