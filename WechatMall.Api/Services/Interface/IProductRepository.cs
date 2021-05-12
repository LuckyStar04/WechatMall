using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public interface IProductRepository
    {
        IQueryable<Product> GetQueryableProducts();
        Task<bool> ProductExistsAsync(string productID);
        Task<Product> GetProductAsync(string productID);

        void AddProduct(string categoryID, Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);

        Task<IEnumerable<ProductImage>> GetProductImagesAsync(string productID);
        ProductImage GetProductImage(Guid guid);
        Task AddProductImage(string productID, ProductImage image);
        void UpdateProductImage(ProductImage image);
        void DeleteProductImage(ProductImage image);
        Task<bool> SaveAsync();
    }
}
