using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MyApiService.Controllers; 
using MyApiService.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("InMemoryDb"); 
});

var app = builder.Build();
// Проверка и заполнение базы данных данными, если она пуста
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!dbContext.Clients.Any()) // Проверяем, есть ли клиенты в базе
    {
        // Заполняем базу начальными данными
        var clients = new Client[]
        {
            new Client { Id = 1, FullName = "John Doe", PhoneNumber = "1234567890" },
            new Client { Id = 2, FullName = "Jane Smith", PhoneNumber = "9876543210" }
        };
        dbContext.Clients.AddRange(clients);
        dbContext.SaveChanges();
    }
    if (!dbContext.ProductType.Any())
    {
        var productTypes = new ProductType[]
        {
            new ProductType { Id = 1, Name = "Electronics" },
            new ProductType { Id = 2, Name = "Clothing" }
        };
        dbContext.ProductType.AddRange(productTypes);
        dbContext.SaveChanges();
    }

    if (!dbContext.Products.Any())
    {
        var products = new Product[]
        {
            new Product { Id = 1, ProductTypeId = 1, Name = "Laptop", Price = 1000, AvailableQuantity = 10 },
            new Product { Id = 2, ProductTypeId = 2, Name = "T-shirt", Price = 20, AvailableQuantity = 50 }
        };
        dbContext.Products.AddRange(products);
        dbContext.SaveChanges();
    }

    if (!dbContext.Orders.Any())
    {
        var orders = new Order[]
        {
            new Order { Id = 1, ClientId = 1, CreationDate = DateTime.Now },
            new Order { Id = 2, ClientId = 2, CreationDate = DateTime.Now }
        };
        dbContext.Orders.AddRange(orders);
        dbContext.SaveChanges();
    }

    if (!dbContext.OrderItems.Any())
    {
        var orderItems = new OrderItem[]
        {
            new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Price = 1000, Quantity = 1 },
            new OrderItem { Id = 2, OrderId = 2, ProductId = 2, Price = 20, Quantity = 2 }
        };
        dbContext.OrderItems.AddRange(orderItems);
        dbContext.SaveChanges();
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); 
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = "swagger"; 
});

app.Run();
