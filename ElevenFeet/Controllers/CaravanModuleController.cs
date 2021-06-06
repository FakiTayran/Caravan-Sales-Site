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
    public class CaravanModuleController : ControllerBase
    {
        private readonly ElevenFeetDbContext elevenFeetDbContext;
        private readonly IFileUpload fileUpload;

        public CaravanModuleController(ElevenFeetDbContext elevenFeetDbContext,IFileUpload fileUpload)
        {
            this.elevenFeetDbContext = elevenFeetDbContext;
            this.fileUpload = fileUpload;
        }
        [HttpPost("CreateCaravanModule")]
        public async Task<CaravanModule> CreateCaravanModule(CaravanModule module,IFormFile file)
        {
            await elevenFeetDbContext.Set<CaravanModule>().AddAsync(module);
            var result = fileUpload.Upload(file);
            if (result.FileResult == Utilities.FileResult.Succeded)
            {
                ModulePictures modulePicture = new ModulePictures();
                modulePicture.pictureUri = result.FileUrl;
                module.ModulePictures = modulePicture;
                await elevenFeetDbContext.SaveChangesAsync();
            }
            await elevenFeetDbContext.SaveChangesAsync();
            return module;
        }

        [HttpPost("DeleteCaravanModule")]
        public async Task DeleteCaravanModule(int id)
        {
            var caravanModule = await elevenFeetDbContext.CaravanModules.FirstOrDefaultAsync(x => x.Id == id);
            elevenFeetDbContext.Set<CaravanModule>().Remove(caravanModule);
            await elevenFeetDbContext.SaveChangesAsync();
        }

        [HttpPost("UpdateCaravanModule")]
        public async Task UpdateCaravanModule(int id)
        {
            var caravanModule = await elevenFeetDbContext.CaravanModules.FirstOrDefaultAsync(x => x.Id == id);
            elevenFeetDbContext.Entry(caravanModule).State = EntityState.Modified;
            await elevenFeetDbContext.SaveChangesAsync();
        }

        [HttpGet("ListCaravanModule")]
        public async Task<List<CaravanModule>> ListCaravanModule()
        {
            return await elevenFeetDbContext.Set<CaravanModule>().ToListAsync();
        }
    }
}

