using System;

namespace StoreApp.Models
{
    public class SaleItem
    {
        public int Id { get; set; }

        public int SaleId { get; set; }
        public Sale Sale { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int QuantitySold { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal InitialPrice { get; set; }

        public string PurchaseCurrency { get; set; }

        public string SaleCurrency { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal Profit
        {
            get
            {
                decimal cost = InitialPrice;

                // تحويل الدولار إلى ليرة إذا لزم
                if (PurchaseCurrency == "USD" && SaleCurrency == "SYP")
                {
                    cost = InitialPrice * ExchangeRate;
                }

                return (SellingPrice - cost) * QuantitySold;
            }
        }
    }
}
