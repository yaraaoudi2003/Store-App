using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApp.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string SaleCurrency { get; set; }

        public decimal ExchangeRate { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.Now;

        public List<SaleItem> Items { get; set; }

        public int? CustomerDebtId { get; set; }
        public CustomerDebt CustomerDebt { get; set; }

  
        public decimal TotalAmount => Items?.Sum(i => i.SellingPrice * i.QuantitySold) ?? 0;

        // فقط للواجهة
        [NotMapped]
        public bool IsDebt { get; set; }

    }
}
