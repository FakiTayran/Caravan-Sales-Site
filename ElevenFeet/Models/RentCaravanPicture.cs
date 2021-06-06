using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenFeet.Models
{
    public class RentCaravanPicture : BaseEntity
    {
        public string ImageUrl { get; set; }

        [ForeignKey("RentCaravan")]
        public int RentCaravanId { get; set; }

        public RentCaravan RentCaravan { get; set; }
    }
}
