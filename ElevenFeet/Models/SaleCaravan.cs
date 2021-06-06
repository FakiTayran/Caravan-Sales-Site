using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenFeet.Models
{
    public class SaleCaravan : BaseEntity
    {
        public SaleCaravan()
        {
            CaravanPictures = new HashSet<SaleCaravanPicture>();
        }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int Kilometer { get; set; }
        public string Gear { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }

        public IEnumerable<SaleCaravanPicture> CaravanPictures { get; set; }
    }
}
