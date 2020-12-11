using ShopBridge.DTO;
using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Interfaces
{
   public interface IInventoryService
    {
        Task<ProductDto> GetProductAsync(int id);
        Task<List<ProductDto>> GetProductsAsync();
        Task<int> AddProductAsync(ProductDto productDto);
        Task<bool> DeleteProductAsync(int id);
    }
}
