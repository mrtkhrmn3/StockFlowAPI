using StockFlowAPI.BusinessLayer.DTOs;

namespace StockFlowAPI.BusinessLayer.Interfaces
{
    public interface IProductService
    {
        Task<bool> CreateAsync(CreateProductDTO dto);
        Task<bool> UpdateAsync(UpdateProductDTO dto);
        Task<bool> DeleteAsync(Guid productId);
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<IEnumerable<ProductDTO>> GetMyProductsAsync();
    }
}
