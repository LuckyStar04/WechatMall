using System.Collections.Generic;
using System.Threading.Tasks;
using WechatMall.Api.DtoParameters;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Services
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryAsync(string categoryID);
        Task<IEnumerable<Category>> GetCategoriesAsync(IEnumerable<string> categoryIDs);
        Task<IEnumerable<Category>> GetCategoriesAsync(CategoryDtoParameter parameter);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        Task<bool> CategoryExistsAsync(string categoryID);

        Task<bool> SaveAsync();
    }
}
