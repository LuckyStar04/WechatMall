using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Dtos;
using WechatMall.Api.Entities;
using WechatMall.Api.Services;

namespace WechatMall.Api.Controllers
{
    [ApiController]
    [Route("/api/shippingfares")]
    public class FareController : ControllerBase
    {
        private readonly IFareRepository fareRepository;
        private readonly IMapper mapper;

        public FareController(IFareRepository fareRepository,
                              IMapper mapper)
        {
            this.fareRepository = fareRepository;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id:int:min(1)}", Name = nameof(GetFare))]
        public async Task<ActionResult<FareDto>> GetFare(int id)
        {
            var fare = await fareRepository.GetFare(id);
            if (fare == null)
            {
                return NotFound();
            }
            var dtoToReturn = mapper.Map<FareDto>(fare);
            return Ok(dtoToReturn);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddFare(FareAddDto fare)
        {
            var fareToAdd = mapper.Map<ShippingFare>(fare);
            fareToAdd.EditTime = DateTime.Now;

            fareRepository.AddFare(fareToAdd);
            await fareRepository.SaveAsync();

            var dtoToReturn = mapper.Map<FareDto>(fareToAdd);
            return CreatedAtRoute(nameof(GetFare), new { id = fareToAdd.Id }, dtoToReturn);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> UpdateFare(int id, FareUpdateDto fare)
        {
            var fareEntity = await fareRepository.GetFare(id);
            if (fareEntity == null)
            {
                return NotFound();
            }

            mapper.Map(fare, fareEntity);
            fareEntity.EditTime = DateTime.Now;

            fareRepository.UpdateFare(fareEntity);
            await fareRepository.SaveAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int:min(1)}")]
        public async Task<IActionResult> PartiallyUpdateFare(int id, JsonPatchDocument<FareUpdateDto> patchDocument)
        {
            var fareEntity = await fareRepository.GetFare(id);
            if (fareEntity == null)
            {
                return NotFound();
            }

            var dtoToPatch = mapper.Map<FareUpdateDto>(fareEntity);
            patchDocument.ApplyTo(dtoToPatch, ModelState);
            if (!TryValidateModel(dtoToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(dtoToPatch, fareEntity);
            fareEntity.EditTime = DateTime.Now;

            fareRepository.UpdateFare(fareEntity);
            await fareRepository.SaveAsync();
            return NoContent();
        }

        //[Authorize(Roles = "Admin")]
        //[HttpDelete("{id:int:min(1)}")]
        //public async Task<IActionResult> DeleteFare(int id)
        //{
        //    var fare = await fareRepository.GetFare(id);
        //    if (fare == null)
        //    {
        //        return NotFound();
        //    }
        //    fareRepository.DeleteFare(fare);
        //    await fareRepository.SaveAsync();
        //    return NoContent();
        //}
    }
}
