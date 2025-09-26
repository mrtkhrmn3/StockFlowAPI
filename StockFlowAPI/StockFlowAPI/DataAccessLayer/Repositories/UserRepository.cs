using Microsoft.EntityFrameworkCore;
using StockFlowAPI.DataAccessLayer.Interfaces;
using StockFlowAPI.Entities;

namespace StockFlowAPI.DataAccessLayer.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly DbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }
    }
}
