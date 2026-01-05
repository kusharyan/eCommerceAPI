namespace eCommerceApi.Models
{
    public class Order
    {
        public int Id {get; set;}
        public int CustomerId {get; set;}
        public DateTime OrderDate{get; set;}
        public List<OrderItem> Items { get; set; }
    }
}