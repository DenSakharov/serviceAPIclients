namespace MyApiService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using MyApiService.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ClientsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Метод добавления клиента
        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] Client client)
        {
            _dbContext.Clients.Add(client);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClient), new { phoneNumber = client.PhoneNumber }, client);
        }

        // Метод получения клиента по номеру телефона
        [HttpGet("{phoneNumber}")]
        public async Task<ActionResult<Client>> GetClient(string phoneNumber)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
            var test = await _dbContext.Clients.CountAsync();
            if (client == null)
            {
                return NotFound();
            }
            return client;
        }

        // Метод получения списка товаров с фильтрацией и сортировкой
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string productType, [FromQuery] bool inStock, [FromQuery] string sort)
        {
            var query = _dbContext.Products.AsQueryable();
            if (!string.IsNullOrEmpty(productType))
            {
                query = query.Where(p => p.ProductType.Name == productType);
            }
            if (inStock)
            {
                query = query.Where(p => p.AvailableQuantity > 0);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(p => p.Price);
                }
                else if (sort.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(p => p.Price);
                }
            }
            var products = await query.ToListAsync();
            return products;
        }

        // Метод получения списка заказов клиента за период
        [HttpGet("{clientId}/orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetClientOrders(int clientId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var orders = await _dbContext.Orders
                .Where(o => o.ClientId == clientId && o.CreationDate >= startDate && o.CreationDate <= endDate)
                .OrderBy(o => o.CreationDate)
                .ToListAsync();
            return orders;
        }

        // Метод формирования заказа
        [HttpPost("{clientId}/orders")]
        public async Task<IActionResult> CreateOrder(int clientId, [FromBody] List<OrderItem> orderItems)
        {
            var client = await _dbContext.Clients.FindAsync(clientId);
            if (client == null)
            {
                return NotFound("Client not found");
            }

            foreach (var orderItem in orderItems)
            {
                var product = await _dbContext.Products.FindAsync(orderItem.ProductId);
                if (product == null)
                {
                    return NotFound($"Product with ID {orderItem.ProductId} not found");
                }

                if (product.AvailableQuantity < orderItem.Quantity)
                {
                    return BadRequest($"Insufficient stock for product {product.Name}");
                }

                var newOrderItem = new OrderItem
                {
                    Product = product,
                    Price = product.Price,
                    Quantity = orderItem.Quantity
                };

                client.Orders.Last().OrderItems.Add(newOrderItem);
                product.AvailableQuantity -= orderItem.Quantity;
            }

            await _dbContext.SaveChangesAsync();
            return Ok("Order created successfully");
        }
    }

}
