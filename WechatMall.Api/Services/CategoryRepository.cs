using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Data;
using WechatMall.Api.DtoParameters;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MallDbContext context;

        public CategoryRepository(MallDbContext context)
        {
            this.context = context;
        }
        public async Task<Category> GetCategoryAsync(string categoryID)
        {
            if (string.IsNullOrWhiteSpace(categoryID)) return default;
            return await context.Categories.FirstOrDefaultAsync(c => categoryID.Equals(c.CategoryID));
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(CategoryDtoParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            var expression = context.Categories
                             .OrderBy(c => c.OrderbyId) as IQueryable<Category>;
            if (parameter.Limit > 0) expression = expression.Take(parameter.Limit);

            return await expression.ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(IEnumerable<string> categoryIDs)
        {
            if (categoryIDs == null) throw new ArgumentNullException(nameof(categoryIDs));

            return await context.Categories
                         .Where(c => categoryIDs.Contains(c.CategoryID))
                         .OrderBy(c => c.OrderbyId)
                         .ToListAsync();
        }

        public void AddCategory(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            context.Categories.Add(category);
        }
        public void UpdateCategory(Category category)
        {
            //No need to write code.
        }
        public void DeleteCategory(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            context.Categories.Remove(category);
        }
        public async Task<bool> CategoryExistsAsync(string categoryID)
        {
            if (string.IsNullOrWhiteSpace(categoryID)) throw new ArgumentNullException(nameof(categoryID));
            return await context.Categories.AnyAsync(c => c.CategoryID.Equals(categoryID));
        }

        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
