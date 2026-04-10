using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;

namespace StoreApp.Models
{
    public class DebtPayment
    {
        public int Id { get; set; }

        public int CustomerDebtId { get; set; }
        [ValidateNever]
        public CustomerDebt? CustomerDebt { get; set; }
        public int? SaleId { get; set; }
        public Sale? Sale { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}