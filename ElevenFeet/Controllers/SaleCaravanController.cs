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
    public class SaleCaravanController : ControllerBase
    {
        private readonly ElevenFeetDbContext elevenFeetDbContext;
        private readonly IFileUpload fileUpload;

        public SaleCaravanController(ElevenFeetDbContext elevenFeetDbContext,IFileUpload fileUpload)
        {
            this.elevenFeetDbContext = elevenFeetDbContext;
            this.fileUpload = fileUpload;
        }
        [HttpPost("CreateSaleCaravan")]
        public async Task<SaleCaravan> CreateSaleCaravanAsync(CaravanSaleDto saleDto)
        {
            SaleCaravan sale = new SaleCaravan
            {
                Brand = saleDto.Brand,
                Capacity = saleDto.Capacity,
                Status = saleDto.Status,
                Description = saleDto.Description,
                Gear = saleDto.Gear,
                Kilometer = saleDto.Kilometer,
                Model = saleDto.Model,
                Price = saleDto.Price,
                Type = saleDto.Type,
                Year = saleDto.Year,
            };   
            await elevenFeetDbContext.Set<SaleCaravan>().AddAsync(sale);
            await elevenFeetDbContext.SaveChangesAsync();
            return sale;
        }

        [HttpGet("DeleteCaravanAsync/{caravanId}")]
        public async Task<SaleCaravan> DeleteCaravanAsync(int caravanId)
        {
            var saleCaravan = await elevenFeetDbContext.SaleCaravans.FirstOrDefaultAsync(x => x.Id == caravanId);
            elevenFeetDbContext.Set<SaleCaravan>().Remove(saleCaravan);
            await elevenFeetDbContext.SaveChangesAsync();
            return saleCaravan;
        }

        [HttpPost("UpdateAsync")]
        public async Task UpdateAsync(int id)
        {
            var saleCaravan = await elevenFeetDbContext.SaleCaravans.FirstOrDefaultAsync(x => x.Id == id);
            elevenFeetDbContext.Entry(saleCaravan).State = EntityState.Modified;
            await elevenFeetDbContext.SaveChangesAsync();
        }

        [HttpGet("ListAllAsync")]
        [AllowAnonymous]
        public async Task<List<SaleCaravan>> ListAllAsync()
        {
            return await elevenFeetDbContext.Set<SaleCaravan>().Include(x=>x.CaravanPictures).ToListAsync();
        }

    }
}