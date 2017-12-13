using System.Data.Entity;
using e_commerce.Models;

namespace e_commerce.DAL
{
    public class StoreContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}