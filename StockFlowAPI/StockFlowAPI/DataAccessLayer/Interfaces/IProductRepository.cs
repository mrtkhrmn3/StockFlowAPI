using StockFlowAPI.Entities;

namespace StockFlowAPI.DataAccessLayer.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetByUserIdAsync(Guid userId);
    }
}
