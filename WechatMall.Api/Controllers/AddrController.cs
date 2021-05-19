using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WechatMall.Api.DtoParameters;
using WechatMall.Api.Dtos;
using WechatMall.Api.Entities;
using WechatMall.Api.Services;

namespace WechatMall.Api.Controllers
{
    [ApiController]
    [Route("api/users/addrs")]
    public class AddrController : ControllerBase
    {
        private readonly IAddrRepository addrRepository;
        private readonly IMapper mapper;

        public AddrController(IAddrRepository addrRepository,
                              IMapper mapper)
        {
            this.addrRepository = addrRepository;
            this.mapper = mapper;
        }

        [Authorize(Roles = "User")]
        [HttpGet(Name = nameof(GetAddrs))]
        public async Task<ActionResult<IEnumerable<AddrDto>>> GetAddrs([FromQuery]AddrDtoParameter parameter)
        {
            Guid userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            IEnumerable<AddrDto> dtoToReturn;
            if (parameter.IsDefault)
            {
                var addr = await addrRepository.GetDefaultAddr(userID);
                if (addr != null)
                {
                    dtoToReturn = new List<AddrDto> { mapper.Map<AddrDto>(addr) };
                }
                else
                {
                    dtoToReturn = Enumerable.Empty<AddrDto>();
                }
            }
            else
            {
                var addrs = await addrRepository.GetAddrsAsync(userID);
                dtoToReturn = mapper.Map<IEnumerable<AddrDto>>(addrs);
            }
            return Ok(dtoToReturn);
        }

        [Authorize(Roles = "User")]
        [HttpGet("{addrId:int:min(1)}", Name = nameof(GetAddr))]
        public async Task<ActionResult<AddrDto>> GetAddr(int addrId)
        {
            Guid userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var addr = await addrRepository.GetAddr(addrId);
            if (addr == null)
            {
                return NotFound();
            }
            if (!addr.UserID.Equals(userID))
            {
                return Unauthorized();
            }
            var dtoToReturn = mapper.Map<AddrDto>(addr);
            return Ok(dtoToReturn);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<ActionResult<AddrDto>> AddAddr(AddrAddDto addr)
        {
            Guid userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var addrToAdd = mapper.Map<ShippingAddr>(addr);
            addrRepository.AddAddr(userID, addrToAdd);
            await addrRepository.SaveAsync();

            var dtoToReturn = mapper.Map<AddrDto>(addrToAdd);
            return CreatedAtRoute(nameof(GetAddr), new { addrId = addrToAdd.Id }, dtoToReturn);
        }

        [Authorize(Roles = "User")]
        [HttpPut("{addrId:int:min(1)}")]
        public async Task<IActionResult> UpdateAddr(int addrId, AddrUpdateDto addr)
        {
            Guid userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var addrEntity = await addrRepository.GetAddr(addrId);
            if (addrEntity == null)
            {
                return NotFound();
            }
            if (!addrEntity.UserID.Equals(userID))
            {
                return Unauthorized();
            }

            mapper.Map(addr, addrEntity);
            addrRepository.UpdateAddr(addrEntity);
            await addrRepository.SaveAsync();
            return NoContent();
        }

        [Authorize(Roles = "User")]
        [HttpPatch("{addrId:int:min(1)}")]
        public async Task<IActionResult> PartiallyUpdateAddr(int addrId, JsonPatchDocument<AddrUpdateDto> patchDocument)
        {
            Guid userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var addrEntity = await addrRepository.GetAddr(addrId);
            if (addrEntity == null)
            {
                return NotFound();
            }
            if (!addrEntity.UserID.Equals(userID))
            {
                return Unauthorized();
            }

            var dtoToPatch = mapper.Map<AddrUpdateDto>(addrEntity);
            patchDocument.ApplyTo(dtoToPatch, ModelState);
            if (!TryValidateModel(dtoToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(dtoToPatch, addrEntity);
            addrRepository.UpdateAddr(addrEntity);
            await addrRepository.SaveAsync();
            return NoContent();
        }

        //[Authorize(Roles = "User")]
        //[HttpDelete("{addrId:int:min(1)}")]
        //public async Task<IActionResult> DeleteAddr(int addrId)
        //{
        //    Guid userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        //    var addrEntity = await addrRepository.GetAddr(addrId);
        //    if (addrEntity == null)
        //    {
        //        return NotFound();
        //    }
        //    if (!addrEntity.UserID.Equals(userID))
        //    {
        //        return Unauthorized();
        //    }
        //    addrRepository.DeleteAddr(addrEntity);
        //    await addrRepository.SaveAsync();
        //    return NoContent();
        //}
    }
}
