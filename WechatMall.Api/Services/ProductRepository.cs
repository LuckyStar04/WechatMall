using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Data;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public class ProductRepository :IProductRepository
    {
        private readonly MallDbContext context;

        public ProductRepository(MallDbContext context)
        {
            this.context = context;
        }
        public IQueryable<Product> GetQueryableProducts()
        {
            return context.Products.Include(p => p.Images);
        }
        public Task<bool> ProductExistsAsync(string productID)
        {
            return context.Products.AnyAsync(p => p.ProductID.Equals(productID));
        }
        public Task<Product> GetProductAsync(string productID)
        {
            if (string.IsNullOrWhiteSpace(productID)) throw new ArgumentNullException(nameof(productID));
            return context.Products
                        .Include(p => p.Images)
                        .Include(p => p.ShippingFare)
                        .FirstOrDefaultAsync(p => p.ProductID.Equals(productID));
        }

        public void AddProduct(string categoryID, Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            product.CategoryID = categoryID;
            context.Products.Add(product);
        }
        public void UpdateProduct(Product product)
        {
            //No need to write code.
        }
        public void DeleteProduct(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            context.Products.Remove(product);
        }

        public async Task<IEnumerable<ProductImage>> GetProductImagesAsync(string productID)
        {
            if (!await context.Products.AnyAsync(p => p.ProductID.Equals(productID)))
            {
                throw new ArgumentException(nameof(productID));
            }

            return await context.ProductImages.Where(p => p.ProductID.Equals(productID)).ToListAsync();
        }

        public ProductImage GetProductImage(Guid guid)
        {
            return context.ProductImages.Include(p => p.Product).Where(p => p.Guid.Equals(guid)).FirstOrDefault();
        }

        public async Task AddProductImage(string productID, ProductImage image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (!await context.Products.AnyAsync(p => p.ProductID.Equals(productID)))
            {
                throw new ArgumentException(nameof(productID));
            }
            image.ProductID = productID;
            await context.ProductImages.AddAsync(image);
        }

        public void UpdateProductImage(ProductImage image)
        {
            //No need write code
        }

        public void DeleteProductImage(ProductImage image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            context.ProductImages.Remove(image);
        }

        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
