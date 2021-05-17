using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using WechatMall.Api.DtoParameters;
using WechatMall.Api.Dtos;
using WechatMall.Api.Entities;
using WechatMall.Api.Helpers;
using WechatMall.Api.Services;

namespace WechatMall.Api.Controllers
{
    /// <summary>
    /// 商品的增删改查。
    /// </summary>
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

#pragma warning disable CS1591
        public ProductController(IProductRepository productRepository,
                                 ICategoryRepository categoryRepository,
                                 IMapper mapper)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }
#pragma warning restore CS1591

        /// <summary>
        /// 查询多条商品信息。
        /// </summary>
        /// <param name="parameter">传入CategoryID、排序、分页等参数</param>
        /// <returns>多条商品信息</returns>
        [Produces("application/json")]
        [AllowAnonymous]
        [HttpGet(Name = nameof(GetProducts))]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] ProductDtoParameter parameter)
        {
            var queryProduct = productRepository.GetQueryableProducts().Where(p => p.OnSale && !p.IsDeleted);
            if (!string.IsNullOrWhiteSpace(parameter.CategoryID))
            {
                queryProduct = queryProduct.Where(p => p.CategoryID.Equals(parameter.CategoryID));
            }
            switch (parameter.OrderBy)
            {
                case OrderType.None:
                    queryProduct = queryProduct.OrderBy(p => p.OrderbyId);
                    break;
                case OrderType.Recommend:
                    queryProduct = queryProduct.Where(p => p.Recommend > 0)
                                               .OrderBy(p => p.Recommend);
                    break;
                case OrderType.TopSales:
                    queryProduct = queryProduct.OrderByDescending(p => p.SoldCount);
                    break;
                default: throw new IndexOutOfRangeException();
            }
                                                
            var pagedProduct = await PagedList<Product>.Create(queryProduct, parameter.PageNumber, parameter.PageSize);

            var previousPageLink = pagedProduct.HasPrevious
                                 ? CreateProductsResourceUri(parameter, ResourceUriType.PreviousPage)
                                 : null;

            var nextPageLink     = pagedProduct.HasNext
                                 ? CreateProductsResourceUri(parameter, ResourceUriType.NextPage)
                                 : null;

            var paginationMetadata = new
            {
                totalCount = pagedProduct.TotalCount,
                pageSize = pagedProduct.PageSize,
                currentPage = pagedProduct.CurrentPage,
                totalPages = pagedProduct.TotalPages,
                previousPageLink,
                nextPageLink
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

            var productDtos = mapper.Map<IEnumerable<ProductDto>>(pagedProduct);
            return Ok(productDtos);
        }

        /// <summary>
        /// 查询单条商品信息。
        /// </summary>
        /// <param name="productID">商品ID，应为10位数字的字符串</param>
        /// <returns>商品详情信息</returns>
        [Produces("application/json")]
        [AllowAnonymous]
        [HttpGet("{productID:length(10)}", Name = nameof(GetProduct))]
        public async Task<ActionResult<ProductDetailDto>> GetProduct(string productID)
        {
            var product = await productRepository.GetProductAsync(productID);
            if (product == null || !product.OnSale || product.IsDeleted) return NotFound();
            var productDetailDto = mapper.Map<ProductDetailDto>(product);
            return Ok(productDetailDto);
        }

        private string CreateProductsResourceUri(ProductDtoParameter parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetProducts), new
                    {
                        CategoryID = parameters.CategoryID,
                        OrderBy = parameters.OrderBy,
                        PageNumber = parameters.PageNumber - 1,
                        PageSize = parameters.PageSize
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetProducts), new
                    {
                        CategoryID = parameters.CategoryID,
                        OrderBy = parameters.OrderBy,
                        PageNumber = parameters.PageNumber + 1,
                        PageSize = parameters.PageSize
                    });
                default:
                    return Url.Link(nameof(GetProducts), new
                    {
                        CategoryID = parameters.CategoryID,
                        OrderBy = parameters.OrderBy,
                        PageNumber = parameters.PageNumber,
                        PageSize = parameters.PageSize
                    });
            }
        }

        private enum ResourceUriType
        {
            PreviousPage,
            NextPage
        }

        /// <summary>
        /// 添加商品信息。
        /// </summary>
        /// <param name="product">要添加的商品信息</param>
        /// <returns>新创建的商品信息</returns>
        /// <response code="201">返回新创建的商品信息</response>
        /// <response code="404">当商品的分类ID所对应分类未找到时</response>
        /// <response code="409">当商品ID冲突重复时</response>
        /// <response code="422">当输入json参数未能成功绑定时</response>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductDetailDto>> AddProduct(ProductAddDto product)
        {
            if (!await categoryRepository.CategoryExistsAsync(product.CategoryID))
            {
                return NotFound("Category not found!");
            }

            if (await productRepository.ProductExistsAsync(product.ProductID))
            {
                return Conflict();
            }
            var productToAdd = mapper.Map<Product>(product);
            productRepository.AddProduct(product.CategoryID, productToAdd);
            await productRepository.SaveAsync();
            var dtoToReturn = mapper.Map<ProductDetailDto>(productToAdd);

            return CreatedAtRoute(nameof(GetProduct), new
            {
                productID = dtoToReturn.Id
            },
            dtoToReturn);
        }

        /// <summary>
        /// 整体更新商品信息。
        /// </summary>
        /// <param name="productID">要更新的商品ID</param>
        /// <param name="product">要更新的商品信息</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{productID:length(10)}")]
        public async Task<IActionResult> UpdateProduct(string productID, ProductUpdateDto product)
        {
            var productEntity = await productRepository.GetProductAsync(productID);
            if (productEntity == null)
            {
                return NotFound();
            }

            mapper.Map(product, productEntity);
            productRepository.UpdateProduct(productEntity);
            await productRepository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// 部分更新单条商品信息。
        /// </summary>
        /// <param name="productID">商品ID，应为10位数字的字符串</param>
        /// <param name="patchDocument"></param>
        /// <returns>商品详情信息</returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{productID:length(10)}")]
        public async Task<IActionResult> PartiallyUpdateProduct(string productID, JsonPatchDocument<ProductUpdateDto> patchDocument)
        {
            var productEntity = await productRepository.GetProductAsync(productID);
            if (productEntity == null)
            {
                return NotFound();
            }
            var dtoToPatch = mapper.Map<ProductUpdateDto>(productEntity);
            patchDocument.ApplyTo(dtoToPatch, ModelState);
            if (!TryValidateModel(dtoToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(dtoToPatch, productEntity);
            productRepository.UpdateProduct(productEntity);
            await productRepository.SaveAsync();
            return NoContent();
        }

        //[Authorize(Roles = "Admin")]
        //[HttpDelete("{productID}")]
        //public async Task<IActionResult> DeleteProduct(string productID)
        //{
        //    var productEntity = await productRepository.GetProductAsync(productID);
        //    if (productEntity == null)
        //    {
        //        return NotFound();
        //    }
        //    productRepository.DeleteProduct(productEntity);
        //    await productRepository.SaveAsync();
        //    return NoContent();
        //}

        public override ActionResult ValidationProblem(ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
