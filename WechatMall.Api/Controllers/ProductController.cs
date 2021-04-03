using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public ProductController(IProductRepository productRepository,
                                 ICategoryRepository categoryRepository,
                                 IMapper mapper)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        [HttpGet(Name = nameof(GetProducts))]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] ProductDtoParameter parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter.CategoryID)) return PagedList<ProductDto>.Empty;
            var queryProduct = productRepository.GetQueryableProducts()
                                                .Where(p => p.CategoryID.Equals(parameter.CategoryID) && p.OnSale && !p.IsDeleted)
                                                .OrderBy(p => p.OrderbyId);
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

        [HttpGet("topSales")]
        public async Task<ActionResult<ProductDto>> GetTopSaleProducts([FromQuery] int limit)
        {
            if (limit < 1) limit = 1;
            var products = await productRepository.GetQueryableProducts()
                                       .Where(p => p.OnSale && !p.IsDeleted)
                                       .OrderByDescending(p => p.SoldCount)
                                       .Take(limit).ToListAsync();
            var productDtos = mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productDtos);

        }

        [HttpGet("{productID}", Name = nameof(GetProduct))]
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
                        PageNumber = parameters.PageNumber - 1,
                        PageSize = parameters.PageSize
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetProducts), new
                    {
                        CategoryID = parameters.CategoryID,
                        PageNumber = parameters.PageNumber + 1,
                        PageSize = parameters.PageSize
                    });
                default:
                    return Url.Link(nameof(GetProducts), new
                    {
                        CategoryID = parameters.CategoryID,
                        PageNumber = parameters.PageNumber,
                        PageSize = parameters.PageSize
                    });
            }
        }
        public enum ResourceUriType
        {
            PreviousPage,
            NextPage
        }

        [HttpPost]
        public async Task<ActionResult<ProductDetailDto>> AddProduct(ProductAddDto product)
        {
            if (!await categoryRepository.CategoryExistsAsync(product.CategoryID))
            {
                return NotFound();
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

        [HttpPut("{productID}")]
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

        [HttpPatch("{productID}")]
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
