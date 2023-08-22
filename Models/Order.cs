namespace MyApiService.Models
{
    public class Order // (Заказ)
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime CreationDate { get; set; }

        // Навигационное свойство для клиента
        public Client Client { get; set; }

        // Навигационное свойство для позиций заказов
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

}
