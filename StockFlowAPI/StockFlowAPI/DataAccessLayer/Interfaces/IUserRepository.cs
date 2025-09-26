using StockFlowAPI.DataAccessLayer.Repositories;
using StockFlowAPI.Entities;

namespace StockFlowAPI.DataAccessLayer.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
    }
}
