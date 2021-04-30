using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.DtoParameters;
using WechatMall.Api.Dtos;
using WechatMall.Api.Entities;
using WechatMall.Api.Services;

namespace WechatMall.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoryController(ICategoryRepository categoryRepository,
                                  IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] CategoryDtoParameter parameter)
        {
            var categories = (await categoryRepository.GetCategoriesAsync(parameter))
                             .Where(c => c.IsShown && !c.IsDeleted);
            var categoryDtos = mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Ok(categoryDtos);
        }

        [AllowAnonymous]
        [HttpGet("{categoryID:length(6)}", Name = nameof(GetCategory))]
        public async Task<ActionResult<CategoryDto>> GetCategory(string categoryID)
        {
            var category = await categoryRepository.GetCategoryAsync(categoryID);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> AddCategory(CategoryAddDto category)
        {
            if (await categoryRepository.CategoryExistsAsync(category.Id))
            {
                return Conflict();
            }
            var categoryToAdd = mapper.Map<Category>(category);
            categoryRepository.AddCategory(categoryToAdd);
            await categoryRepository.SaveAsync();
            var dtoToReturn = mapper.Map<CategoryDto>(categoryToAdd);

            return CreatedAtRoute(nameof(GetCategory), new
            {
                categoryID = dtoToReturn.Id
            },
            dtoToReturn);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{categoryID:length(6)}")]
        public async Task<IActionResult> UpdateCategory(string categoryID, CategoryUpdateDto category)
        {
            var categoryEntity = await categoryRepository.GetCategoryAsync(categoryID);
            if (categoryEntity == null)
            {
                return NotFound();
            }
            mapper.Map(category, categoryEntity);
            categoryRepository.UpdateCategory(categoryEntity);
            await categoryRepository.SaveAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{categoryID:length(6)}")]
        public async Task<IActionResult> PartiallyUpdateCategory(string categoryID, JsonPatchDocument<CategoryUpdateDto> patchDocument)
        {
            var categoryEntity = await categoryRepository.GetCategoryAsync(categoryID);
            if (categoryEntity == null)
            {
                return NotFound();
            }
            var dtoToPatch = mapper.Map<CategoryUpdateDto>(categoryEntity);

            patchDocument.ApplyTo(dtoToPatch, ModelState);
            if (!TryValidateModel(dtoToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(dtoToPatch, categoryEntity);
            categoryRepository.UpdateCategory(categoryEntity);
            await categoryRepository.SaveAsync();
            return NoContent();
        }

        //[Authorize(Roles = "Admin")]
        //[HttpDelete("{categoryID}")]
        //public async Task<IActionResult> DeleteCategory(string categoryID)
        //{
        //    var categoryEntity = await categoryRepository.GetCategoryAsync(categoryID);
        //    if (categoryEntity == null)
        //    {
        //        return NotFound();
        //    }
        //    categoryRepository.DeleteCategory(categoryEntity);
        //    await categoryRepository.SaveAsync();
        //    return NoContent();
        //}

        public override ActionResult ValidationProblem(ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
