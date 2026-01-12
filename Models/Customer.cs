namespace SampleRestAPI.Models
{
    public class Customer
    {
        public long CustomerId { get; set; }
        public string? Name { get; set; }
        public List<Order>? Orders { get; set; }
    }
}