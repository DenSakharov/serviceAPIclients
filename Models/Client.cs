namespace MyApiService.Models
{
    public class Client //(Клиент)
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        // Навигационное свойство для заказов клиента
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

}
