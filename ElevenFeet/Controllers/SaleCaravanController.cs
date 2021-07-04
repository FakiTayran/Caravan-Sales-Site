using ElevenFeet.Dtos;
using ElevenFeet.Models;
using ElevenFeet.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IWebHostEnvironment env;

        public SaleCaravanController(ElevenFeetDbContext elevenFeetDbContext, IFileUpload fileUpload, IWebHostEnvironment env)
        {
            this.elevenFeetDbContext = elevenFeetDbContext;
            this.fileUpload = fileUpload;
            this.env = env;
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
        public async Task<SaleCaravan> UpdateAsync(SaleCaravan saleCaravan)
        {
            var editedCaravan = await elevenFeetDbContext.SaleCaravans.FirstOrDefaultAsync(x => x.Id == saleCaravan.Id);
            editedCaravan.Kilometer = saleCaravan.Kilometer;
            editedCaravan.Model = saleCaravan.Model;
            editedCaravan.Price = saleCaravan.Price;
            editedCaravan.Status = saleCaravan.Status;
            editedCaravan.Type = saleCaravan.Type;
            editedCaravan.Year = saleCaravan.Year;
            editedCaravan.Description = saleCaravan.Description;
            editedCaravan.Brand = saleCaravan.Brand;
            editedCaravan.Capacity = saleCaravan.Capacity;
            editedCaravan.Gear = saleCaravan.Gear;
            elevenFeetDbContext.Entry(editedCaravan).State = EntityState.Modified;
            await elevenFeetDbContext.SaveChangesAsync();
            return editedCaravan;
        }

        [HttpGet("ListAllAsync")]
        [AllowAnonymous]
        public async Task<List<SaleCaravan>> ListAllAsync()
        {
            return await elevenFeetDbContext.Set<SaleCaravan>().Include(x => x.CaravanPictures).ToListAsync();
        }

        [HttpGet("GetPicture/{id}")]
        [AllowAnonymous]
        public async Task<List<SaleCaravanPicture>> GetPicture(int id)
        {
            return await elevenFeetDbContext.SaleCaravanPictures.Where(x => x.SaleCaravanId == id).ToListAsync();
        }

        [HttpGet("DeleteImage/{id}")]
        [AllowAnonymous]
        public async Task DeleteImage(int id)
        {
            var image = await elevenFeetDbContext.SaleCaravanPictures.FirstOrDefaultAsync(x => x.Id == id);
            elevenFeetDbContext.Entry(image).State = EntityState.Deleted;
            await elevenFeetDbContext.SaveChangesAsync();
            if (System.IO.File.Exists(image.ImageUrl))
            {
                System.IO.File.Delete(image.ImageUrl);
            }
        }
    }
}