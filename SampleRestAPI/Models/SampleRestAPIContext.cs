using Microsoft.EntityFrameworkCore;

namespace SampleRestAPI.Models
{
    public class SampleRestAPIContext : DbContext
    {
        public SampleRestAPIContext(DbContextOptions<SampleRestAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } = null!;
    }
}