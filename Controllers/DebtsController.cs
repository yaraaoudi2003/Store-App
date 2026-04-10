using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data;
using StoreApp.Models;
using System.Linq;

namespace StoreApp.Controllers
{
    public class DebtsController : Controller
    {
        private readonly AppDbContext _context;

        public DebtsController(AppDbContext context)
        {
            _context = context;
        }

        // عرض الديون
        public IActionResult Index()
        {
            var debts = _context.CustomerDebts
                .Include(d => d.Sales)
                .ThenInclude(s => s.Items)
                .Include(d => d.Payments)
                .OrderByDescending(d => d.Date)
                .ToList();

            return View(debts);
        }

        // تفاصيل الدين
        public IActionResult Details(int id)
        {
            var debt = _context.CustomerDebts
                .Include(d => d.Sales)
                    .ThenInclude(s => s.Items)
                .Include(d => d.Payments)
                .FirstOrDefault(d => d.Id == id);

            if (debt == null)
                return NotFound();

            return View(debt);
        }

        // إضافة دفعة
        public IActionResult AddPayment(int debtId, int saleId)
        {
            ViewBag.DebtId = debtId;
            ViewBag.SaleId = saleId;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPayment(DebtPayment payment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DebtId = payment.CustomerDebtId;
                ViewBag.SaleId = payment.SaleId;
                return View(payment);
            }

            if (payment.CustomerDebtId == 0 || payment.SaleId == null)
            {
                return Content("حدث خطأ: لم يتم تحديد الفاتورة أو الدين");
            }

            // 🔥 جلب الفاتورة نفسها (مو كل الديون)
            var sale = _context.Sales
                .Include(s => s.Items)
                .Include(s => s.CustomerDebt)
                .ThenInclude(d => d.Payments)
                .FirstOrDefault(s => s.Id == payment.SaleId);

            if (sale == null)
            {
                return Content("حدث خطأ: لم يتم العثور على الفاتورة");
            }

            //  إجمالي الفاتورة
            var invoiceTotal = sale.Items.Sum(i => i.SellingPrice * i.QuantitySold);

            //  المدفوع فقط لهذه الفاتورة
            var paid = sale.CustomerDebt.Payments
                .Where(p => p.SaleId == sale.Id)
                .Sum(p => p.Amount);

            var remaining = invoiceTotal - paid;

            //  تحقق من المبلغ
            if (payment.Amount > remaining)
            {
                ModelState.AddModelError("", $"المبلغ أكبر من المتبقي ({remaining.ToString("N2")})");
                ViewBag.DebtId = payment.CustomerDebtId;
                ViewBag.SaleId = payment.SaleId;
                return View(payment);
            }

            //  حفظ الدفع مع ربطه بالفاتورة
            payment.Id = 0;
            payment.Date = DateTime.Now;

            _context.DebtPayments.Add(payment);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = payment.CustomerDebtId });
        }
    }
}