using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WechatMall.Api.Entities;
using WechatMall.Api.Services;

namespace WechatMall.Api.Controllers
{
    [ApiController]
    [Route("/api/configs")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigRepository repository;

        public ConfigController(IConfigRepository repository)
        {
            this.repository = repository;
        }

        [AllowAnonymous]
        [HttpGet("{key}", Name = nameof(GetConfig))]
        public async Task<string> GetConfig(string key)
        {
            var config = await repository.GetConfig(key);
            if (config == null)
            {
                Response.StatusCode = 404;
                return string.Empty;
            }
            return config.Value;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<string>> AddConfig(SiteConfig config)
        {
            if (await repository.ConfigExistsAsync(config.Key))
            {
                return Conflict();
            }
            repository.AddConfig(config);
            await repository.SaveAsync();
            return CreatedAtRoute(nameof(GetConfig), new { config.Key }, config.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{key}")]
        public async Task<IActionResult> UpdateConfig(string key, SiteConfig config)
        {
            var configEntity = await repository.GetConfig(key);
            if (configEntity == null)
            {
                return NotFound();
            }
            configEntity.Key = config.Key;
            configEntity.Value = config.Value;
            repository.UpdateConfig(configEntity);
            await repository.SaveAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteConfig(string key)
        {
            var configEntity = await repository.GetConfig(key);
            if (configEntity == null)
            {
                return NotFound();
            }
            repository.RemoveConfig(configEntity);
            await repository.SaveAsync();
            return NoContent();
        }
    }
}
