namespace MyApiService.Models
{
    public class Product //(Товар)
    {
        public int Id { get; set; }
        public int ProductTypeId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int AvailableQuantity { get; set; }

        // Навигационное свойство для типа товара
        public ProductType ProductType { get; set; }

        // Навигационное свойство для позиций заказов
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

}
