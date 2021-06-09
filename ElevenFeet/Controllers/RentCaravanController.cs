using ElevenFeet.Dtos;
using ElevenFeet.Models;
using ElevenFeet.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenFeet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentCaravanController : ControllerBase
    {
        private readonly ElevenFeetDbContext elevenFeetDbContext;
        private readonly IFileUpload fileUpload;

        public RentCaravanController(ElevenFeetDbContext elevenFeetDbContext, IFileUpload fileUpload)
        {
            this.elevenFeetDbContext = elevenFeetDbContext;
            this.fileUpload = fileUpload;
        }
        [HttpPost("CreateRentCaravan")]
        public async Task<RentCaravan> CreateRentCaravan(CaravanRentDto rentDto)
        {
            RentCaravan rent = new RentCaravan
            {
                Brand = rentDto.Brand,
                Capacity = rentDto.Capacity,
                Status = rentDto.Status,
                Description = rentDto.Description,
                Gear = rentDto.Gear,
                Kilometer = rentDto.Kilometer,
                Model = rentDto.Model,
                Price = rentDto.Price,
                Type = rentDto.Type,
                Year = rentDto.Year,
            };
            await elevenFeetDbContext.Set<RentCaravan>().AddAsync(rent);
            await elevenFeetDbContext.SaveChangesAsync();
            return rent;
        }

        [HttpGet("DeleteCaravanAsync/{caravanId}")]
        public async Task<RentCaravan> DeleteCaravanAsync(int caravanId)
        {
            var rentCaravan = await elevenFeetDbContext.RentCaravans.FirstOrDefaultAsync(x => x.Id == caravanId);
            elevenFeetDbContext.Set<RentCaravan>().Remove(rentCaravan);
            await elevenFeetDbContext.SaveChangesAsync();
            return rentCaravan;
        }

        [HttpPost("UpdateAsync")]
        public async Task UpdateAsync(int id)
        {
            var rentCaravan = await elevenFeetDbContext.RentCaravans.FirstOrDefaultAsync(x => x.Id == id);
            elevenFeetDbContext.Entry(rentCaravan).State = EntityState.Modified;
            await elevenFeetDbContext.SaveChangesAsync();
        }

        [HttpGet("ListAllAsync")]
        [AllowAnonymous]
        public async Task<List<RentCaravan>> ListAllAsync()
        {
            return await elevenFeetDbContext.Set<RentCaravan>().Include(x => x.CaravanPictures).ToListAsync();
        }
    }
}
