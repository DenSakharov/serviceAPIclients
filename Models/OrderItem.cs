namespace MyApiService.Models
{
    public class OrderItem //(Позиция заказа)
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Навигационные свойства для заказа и товара
        public Order Order { get; set; }
        public Product Product { get; set; }
    }

}
