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
    public class RentCaravanPictureUpload : ControllerBase
    {
        private readonly ElevenFeetDbContext elevenFeetDbContext;
        private readonly IFileUpload fileUpload;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RentCaravanPictureUpload(ElevenFeetDbContext elevenFeetDbContext, IFileUpload fileUpload, IHttpContextAccessor httpContextAccessor)
        {
            this.elevenFeetDbContext = elevenFeetDbContext;
            this.fileUpload = fileUpload;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("Upload/{rentCaravanId}")]
        public async Task<IActionResult> Upload(IFormCollection file, int rentCaravanId)  //Defined word "file" in dropzone
        {
            var rentCaravan = await elevenFeetDbContext.RentCaravans.Include(x => x.CaravanPictures).FirstOrDefaultAsync(x => x.Id == rentCaravanId);
            if (rentCaravan == null)
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
                        RentCaravanPicture rentCaravanPicture = new RentCaravanPicture();
                        await elevenFeetDbContext.Set<RentCaravanPicture>().AddAsync(rentCaravanPicture);
                        rentCaravanPicture.ImageUrl = System.IO.Path.Combine($"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host.Value}", "Images", result.FileUrl);
                        rentCaravanPicture.RentCaravanId = rentCaravan.Id;
                        rentCaravan.CaravanPictures.ToList().Add(rentCaravanPicture);
                    }
                }
                await elevenFeetDbContext.SaveChangesAsync();
            }
            return Ok();
        }
    }
}
