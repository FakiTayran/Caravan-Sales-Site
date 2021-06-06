using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenFeet.Dtos
{
    public class CaravanSaleDto
    {
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

        public IFormFile[] Files { get; set; }
    }
}
