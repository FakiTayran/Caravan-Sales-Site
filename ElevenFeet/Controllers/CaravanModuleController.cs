using ElevenFeet.Models;
using ElevenFeet.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenFeet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CaravanModuleController : ControllerBase
    {
        private readonly ElevenFeetDbContext elevenFeetDbContext;
        private readonly IFileUpload fileUpload;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CaravanModuleController(ElevenFeetDbContext elevenFeetDbContext,IFileUpload fileUpload, IHttpContextAccessor httpContextAccessor)
        {
            this.elevenFeetDbContext = elevenFeetDbContext;
            this.fileUpload = fileUpload;
            this.httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("CreateCaravanModule/{Name}/{Price}/{Description}")]
        public async Task<CaravanModule> CreateCaravanModule(IFormFile file,string Name,decimal Price,string Description)
        { 
            var module = new CaravanModule
            {
                Name = Name,
                Price = Price,
                Description = Description
            };
            await elevenFeetDbContext.Set<CaravanModule>().AddAsync(module);
            var result = fileUpload.Upload(file);
            if (result.FileResult == Utilities.FileResult.Succeded)
            {
                ModulePictures modulePicture = new ModulePictures();
                modulePicture.pictureUri = System.IO.Path.Combine($"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host.Value}", "Images", result.FileUrl);
                module.ModulePicture = modulePicture;
                await elevenFeetDbContext.SaveChangesAsync();
            }
            return module;
        } 

        [HttpGet("DeleteCaravanModule/{id}")]
        public async Task<CaravanModule> DeleteCaravanModule(int id)
        {
            var caravanModule = await elevenFeetDbContext.CaravanModules.FirstOrDefaultAsync(x => x.Id == id);
            elevenFeetDbContext.Set<CaravanModule>().Remove(caravanModule);
            await elevenFeetDbContext.SaveChangesAsync();
            return caravanModule;
        }

        [HttpPost("UpdateCaravanModule")]
        public async Task UpdateCaravanModule(int id)
        {
            var caravanModule = await elevenFeetDbContext.CaravanModules.FirstOrDefaultAsync(x => x.Id == id);
            elevenFeetDbContext.Entry(caravanModule).State = EntityState.Modified;
            await elevenFeetDbContext.SaveChangesAsync();
        }

        [HttpGet("ListCaravanModule")]
        [AllowAnonymous]
        public async Task<List<CaravanModule>> ListCaravanModule()
        {
            return await elevenFeetDbContext.Set<CaravanModule>().Include(x=>x.ModulePicture).ToListAsync();
        }
    }
}

