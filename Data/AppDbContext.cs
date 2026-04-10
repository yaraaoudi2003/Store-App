using Microsoft.EntityFrameworkCore;
using StoreApp.Models;

namespace StoreApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
		public DbSet<SaleItem> SaleItem { get; set; }

		public DbSet<CustomerDebt> CustomerDebts { get; set; }

        public DbSet<DebtItem> DebtItems { get; set; }

        public DbSet<DebtPayment> DebtPayments { get; set; }
    }
}