namespace MyApiService.Models
{
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Создание начальных данных для клиентов
            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, FullName = "John Doe", PhoneNumber = "1234567890" },
                new Client { Id = 2, FullName = "Jane Smith", PhoneNumber = "9876543210" }
            );
            // Создание начальных данных для типов товаров
            modelBuilder.Entity<ProductType>().HasData(
                new ProductType { Id = 1, Name = "Electronics" },
                new ProductType { Id = 2, Name = "Clothing" }
            );

            // Создание начальных данных для товаров
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, ProductTypeId = 1, Name = "Laptop", Price = 1000, AvailableQuantity = 10 },
                new Product { Id = 2, ProductTypeId = 2, Name = "T-shirt", Price = 20, AvailableQuantity = 50 }
            );

            // Создание начальных данных для заказов
            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, ClientId = 1, CreationDate = DateTime.Now },
                new Order { Id = 2, ClientId = 2, CreationDate = DateTime.Now }
            );

            // Создание начальных данных для позиций заказов
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Price = 1000, Quantity = 1 },
                new OrderItem { Id = 2, OrderId = 2, ProductId = 2, Price = 20, Quantity = 2 }
            );

          
        }
    }
}
