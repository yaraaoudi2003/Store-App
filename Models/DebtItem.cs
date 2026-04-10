using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreApp.Models
{
    public class DebtItem
    {

        public int Id { get; set; }

        public int CustomerDebtId { get; set; }
        public CustomerDebt? CustomerDebt { get; set; }
		[Required]
		public int ProductId { get; set; }

		[ValidateNever]
		public Product? Product { get; set; }

      

        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public string Currency { get; set; }
        [NotMapped]

		public decimal Total => Quantity * Price;

    }
}