using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StoreApp.Models
{
    public class CustomerDebt
    {
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        //  الجديد
        public List<Sale> Sales { get; set; } = new();

        public List<DebtPayment> Payments { get; set; } = new();

        //  الحساب من الفواتير
        public decimal TotalAmount
        {
            get
            {
                return Sales?.Sum(s => s.TotalAmount) ?? 0;
            }
        }

        public decimal PaidAmount
        {
            get
            {
                return Payments?.Sum(x => x.Amount) ?? 0;
            }
        }

        public decimal Remaining
        {
            get
            {
                return TotalAmount - PaidAmount;
            }
        }
        public string Status =>
            Remaining == 0 ? "مدفوع" :
            PaidAmount > 0 ? "جزئي" : "غير مدفوع";
    }
}