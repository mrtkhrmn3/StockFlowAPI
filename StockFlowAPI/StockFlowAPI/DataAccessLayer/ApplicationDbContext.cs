using Microsoft.EntityFrameworkCore;
using StockFlowAPI.Entities;

namespace StockFlowAPI.DataAccessLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
