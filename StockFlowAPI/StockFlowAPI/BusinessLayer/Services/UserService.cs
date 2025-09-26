using Microsoft.IdentityModel.Tokens;
using StockFlowAPI.BusinessLayer.DTOs;
using StockFlowAPI.BusinessLayer.Interfaces;
using StockFlowAPI.DataAccessLayer.Interfaces;
using StockFlowAPI.Entities;
using StockFlowAPI.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockFlowAPI.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public async Task<bool> RegisterAsync(RegisterDTO dto)
        {
            if(dto.Password != dto.PasswordAgain)
            {
                return false;
            }

            var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
            {
                return false;
            }
 ;
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Surname = dto.Surname,
                Username = dto.Username,
                Password = PasswordHelper.HashPassword(dto.Password)
            };

            await _userRepository.AddAsync(user);
            return await _userRepository.SaveChangesAsync();
        }
        public async Task<LoginResponseDTO?> LoginAsync(LoginDTO dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null) return null;

            if (!PasswordHelper.VerifyPassword(dto.Password, user.Password))
                return null;

            // Access Token üret
            var accessToken = GenerateAccessToken(user, out DateTime expiration);

            // Refresh Token üret
            var refreshToken = GenerateRefreshToken();

            // DB’ye kaydet
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return new LoginResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = expiration
            };
        }

        public async Task<LoginResponseDTO?> RefreshTokenAsync(RefreshRequestDTO dto)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var user = await _userRepository.GetByRefreshTokenAsync(dto.RefreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return null;

            // Yeni tokenlar üret
            var accessToken = GenerateAccessToken(user, out DateTime expiration);
            var refreshToken = GenerateRefreshToken();

            // DB güncelle
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(Convert.ToDouble(jwtSettings["RefreshTokenDays"]));
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return new LoginResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = expiration
            };
        }

        private string GenerateAccessToken(User user, out DateTime expiration)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("UserId", user.Id.ToString())
            };

            expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"]));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
