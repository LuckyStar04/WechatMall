using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Dtos;
using WechatMall.Api.Entities;
using WechatMall.Api.Services;

namespace WechatMall.Api.Controllers
{
    [ApiController]
    [Route("api/products/{productID:length(10)}/images")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IWebHostEnvironment environment;
        private readonly IMapper mapper;

        public ProductImageController(IProductRepository productRepository,
                                      IWebHostEnvironment environment,
                                      IMapper mapper)
        {
            this.productRepository = productRepository;
            this.environment = environment;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet(Name = nameof(GetProductImages))]
        public async Task<ActionResult<IEnumerable<ProductImageDto>>> GetProductImages(string productID)
        {
            if (!await productRepository.ProductExistsAsync(productID)) return NotFound();
            var images = await productRepository.GetProductImagesAsync(productID);
            var dtoToReturn = mapper.Map<IEnumerable<ProductImageDto>>(images);
            return Ok(dtoToReturn);
        }

        [AllowAnonymous]
        [HttpGet("{guid:guid}", Name = nameof(GetProductImage))]
        public ActionResult<ProductImageDto> GetProductImage(Guid guid)
        {
            var image = productRepository.GetProductImage(guid);
            if (image == null) return NotFound();
            var dtoToReturn = mapper.Map<ProductImageDto>(image);
            return Ok(dtoToReturn);
        }

        private Task CompressAndSaveImage(Image image, string path)
        {
            return Task.Run(() =>
            {
                image.Mutate(x => x.AutoOrient());

                if (image.Width > image.Height)
                {
                    if (image.Width > 800)
                        image.Mutate(x => x.Resize(800, image.Height * 800 / image.Width));
                }
                else
                {
                    if (image.Height > 800)
                        image.Mutate(x => x.Resize(image.Width * 800 / image.Height, 800));
                }
                image.SaveAsJpeg(path);
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProductImages(string productID, IFormFileCollection files)
        {
            var product = await productRepository.GetProductAsync(productID);
            if (product == null)
            {
                return NotFound();
            }
            if (files.Count() > 5)
            {
                return UnprocessableEntity();
            }
            else
            {
                List<Image> images = new List<Image>(5);

                try
                {
                    foreach (var file in files)
                    {
                        var fileName = file.FileName.ToLower();
                        if (!fileName.EndsWith(".jpg") && !fileName.EndsWith(".jpeg") && !fileName.EndsWith(".png") && !fileName.EndsWith(".gif"))
                        {
                            ModelState.AddModelError("", "Image format must be JPG/JPEG/PNG/GIF.");
                            return ValidationProblem(ModelState);
                        }
                        images.Add(await Image.LoadAsync(file.OpenReadStream()));
                    }

                    foreach (var image in images)
                    {
                        var newGuid = Guid.NewGuid();
                        var realPath = $"{environment.WebRootPath}\\products\\{product.CategoryID}\\{productID}\\{newGuid}.jpg";
                        var dir = $"{environment.WebRootPath}\\products\\{product.CategoryID}\\{productID}\\";
                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                        await CompressAndSaveImage(image, realPath);

                        await productRepository.AddProductImage(productID, new ProductImage
                        {
                            Guid = newGuid,
                            ImagePath = $"http://{this.Request.Host}/products/{product.CategoryID}/{productID}/{newGuid}.jpg",
                            PhysicalPath = realPath,
                            ProductID = productID,
                            OrderbyId = productRepository.GetQueryableProducts()
                                                         .Where(p => p.ProductID.Equals(productID))
                                                         .FirstOrDefault()?.Images
                                                         .Max(i => i.OrderbyId) + 100 ?? 100,
                        });
                        await productRepository.SaveAsync();
                    }
                    return NoContent();
                }
                catch (UnknownImageFormatException)
                {
                    ModelState.AddModelError("", "Unknown image format.");
                    return ValidationProblem(ModelState);
                }
                finally
                {
                    foreach (var image in images) image.Dispose();
                }
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{guid:guid}")]
        public async Task<IActionResult> UpdateImage(Guid guid, IFormFile file)
        {
            var imageEntity = productRepository.GetProductImage(guid);
            var product = imageEntity.Product;
            var productID = product.ProductID;
            if (imageEntity == null)
            {
                return NotFound();
            }

            try
            {
                var fileName = file.FileName.ToLower();
                if (!fileName.EndsWith(".jpg") && !fileName.EndsWith(".jpeg") && !fileName.EndsWith(".png") && !fileName.EndsWith(".gif"))
                {
                    ModelState.AddModelError("", "Image format must be JPG/JPEG/PNG/GIF.");
                    return ValidationProblem(ModelState);
                }
                Image image = await Image.LoadAsync(file.OpenReadStream());

                var newGuid = Guid.NewGuid();
                var oldPath = imageEntity.PhysicalPath;
                var newPath = $"{environment.WebRootPath}\\products\\{product.CategoryID}\\{productID}\\{newGuid}.jpg";
                var dir = $"{environment.WebRootPath}\\products\\{product.CategoryID}\\{productID}\\";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                await CompressAndSaveImage(image, newPath);

                //更新Entity
                imageEntity.Guid = newGuid;
                imageEntity.ImagePath = $"http://{this.Request.Host}/products/{product.CategoryID}/{productID}/{newGuid}.jpg";
                imageEntity.PhysicalPath = newPath;
                await productRepository.SaveAsync();

                //更新成功删除旧文件
                System.IO.File.Delete(oldPath);
                return NoContent();
            }
            catch (UnknownImageFormatException)
            {
                ModelState.AddModelError("", "Unknown image format.");
                return ValidationProblem(ModelState);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{guid:guid}")]
        public async Task<IActionResult> DeleteProductImage(Guid guid)
        {
            var imageEntity = productRepository.GetProductImage(guid);
            var physicalPath = imageEntity.PhysicalPath;
            if (imageEntity == null)
            {
                return NotFound();
            }
            productRepository.DeleteProductImage(imageEntity);
            await productRepository.SaveAsync();
            System.IO.File.Delete(physicalPath);
            return NoContent();
        }

        public override ActionResult ValidationProblem(ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
