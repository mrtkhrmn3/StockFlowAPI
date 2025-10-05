using StockFlowAPI.BusinessLayer.DTOs;
using StockFlowAPI.BusinessLayer.Interfaces;
using StockFlowAPI.DataAccessLayer.Interfaces;
using StockFlowAPI.Entities;
using Microsoft.AspNetCore.Http;

namespace StockFlowAPI.BusinessLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        // JWT'den UserId çekme
        private Guid GetUserIdFromJwt()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new Exception("UserId not found in JWT");
            return Guid.Parse(userIdClaim);
        }

        public async Task<bool> CreateAsync(CreateProductDTO dto)
        {
            var userId = GetUserIdFromJwt();

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity,
                PurchaseDate = dto.PurchaseDate,
                Company = dto.Company,
                UserId = userId
            };

            await _productRepository.AddAsync(product);
            return await _productRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UpdateProductDTO dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.Id);
            if (product == null)
                return false; // ürün yoksa

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Quantity = dto.Quantity;
            product.PurchaseDate = dto.PurchaseDate;
            product.Company = dto.Company;

            _productRepository.Update(product);
            return await _productRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return false;

            _productRepository.Delete(product);
            return await _productRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDTO>> GetMyProductsAsync()
        {
            var userId = GetUserIdFromJwt();
            var products = await _productRepository.GetByUserIdAsync(userId);

            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                PurchaseDate = p.PurchaseDate,
                Company = p.Company
            });
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                PurchaseDate = p.PurchaseDate,
                Company = p.Company
            });
        }
    }
}
