using Microsoft.EntityFrameworkCore;
using StockFlowAPI.DataAccessLayer.Interfaces;
using StockFlowAPI.Entities;

namespace StockFlowAPI.DataAccessLayer.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Products
                                 .Where(p => p.UserId == userId)
                                 .ToListAsync();
        }
    }
}
