using ElevenFeet.Models;
using ElevenFeet.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class SaleCaravanPictureUpload : ControllerBase
    {
        private readonly ElevenFeetDbContext elevenFeetDbContext;
        private readonly IFileUpload fileUpload;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SaleCaravanPictureUpload(ElevenFeetDbContext elevenFeetDbContext, IFileUpload fileUpload, IHttpContextAccessor httpContextAccessor)
        {
            this.elevenFeetDbContext = elevenFeetDbContext;
            this.fileUpload = fileUpload;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("Upload/{saleCaravanId}")]
        public async Task<IActionResult> Upload(IFormCollection file, int saleCaravanId)  //Defined word "file" in dropzone
        {
            var saleCaravan = await elevenFeetDbContext.SaleCaravans.Include(x => x.CaravanPictures).FirstOrDefaultAsync(x => x.Id == saleCaravanId);
            if (saleCaravan == null)
            {
                return NotFound();
            }
            if (file.Files.Count() > 0)
            {
                foreach (IFormFile item in file.Files)
                {
                    var result = fileUpload.Upload(item);
                    if (result.FileResult == Utilities.FileResult.Succeded)
                    {
                        SaleCaravanPicture saleCaravanPicture = new SaleCaravanPicture();
                        await elevenFeetDbContext.Set<SaleCaravanPicture>().AddAsync(saleCaravanPicture);
                        saleCaravanPicture.ImageUrl = System.IO.Path.Combine($"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host.Value}","Images", result.FileUrl);
                        saleCaravanPicture.SaleCaravanId = saleCaravan.Id;
                        saleCaravan.CaravanPictures.ToList().Add(saleCaravanPicture);
                    }
                }
                await elevenFeetDbContext.SaveChangesAsync();
            }
            return Ok(file);
        }
    }
}

