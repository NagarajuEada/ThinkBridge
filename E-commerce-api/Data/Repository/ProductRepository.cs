using ShopBridge.Data.DataContext;
using ShopBridge.Interfaces.Repository;
using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Data.Repository
{
    public class ProductRepository:GenericRepository<Product, InventoryDbContext>,IProductRepository
    {
    }
}
