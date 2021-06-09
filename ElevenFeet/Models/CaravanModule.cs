using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenFeet.Models
{
    public class CaravanModule : BaseEntity
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        [ForeignKey("ModulePicture")]
        public int ModulePictureId { get; set; }

        public ModulePictures ModulePicture { get; set; }
    }
}
