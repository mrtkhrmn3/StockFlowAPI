using StockFlowAPI.BusinessLayer.DTOs;
using StockFlowAPI.Entities;

namespace StockFlowAPI.BusinessLayer.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(RegisterDTO dto);
        Task<LoginResponseDTO?> LoginAsync(LoginDTO dto);
        Task<LoginResponseDTO?> RefreshTokenAsync(RefreshRequestDTO dto);
    }
}
