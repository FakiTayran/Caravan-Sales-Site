using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenFeet.Models
{
    public class SaleCaravanPicture : BaseEntity
    {
        public string ImageUrl { get; set; }

        [ForeignKey("SaleCaravan")]
        public int SaleCaravanId { get; set; }

        public SaleCaravan SaleCaravan { get; set; }
    }
}
