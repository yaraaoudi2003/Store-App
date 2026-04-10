using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال اسم المنتج")]
        public string Name { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال الكمية")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال السعر البدائي")]
        public decimal initialPrice { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال سعر البيع لكل وحدة")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "الرجاء تحديد العملة")]
        public string Currency { get; set; } // $ أو ل.س

        public decimal TotalPrice => initialPrice * Quantity;

        public DateTime Date { get; set; } = DateTime.Now;
    }
}