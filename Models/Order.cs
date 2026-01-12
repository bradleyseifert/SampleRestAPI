using System.ComponentModel.DataAnnotations;

namespace SampleRestAPI.Models
{
    public class Order
    {
        [Key]
        public long OrderNumber { get; set; }
        public string? ProductName { get; set; }
    }
}