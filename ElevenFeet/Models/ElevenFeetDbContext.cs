using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ElevenFeet.Models
{
    public class ElevenFeetDbContext : IdentityDbContext<IdentityUser>
    {
        public ElevenFeetDbContext(DbContextOptions<ElevenFeetDbContext> options) : base(options)
        {

        }

        public DbSet<SaleCaravan> SaleCaravans { get; set; }
        public DbSet<SaleCaravanPicture> SaleCaravanPictures { get; set; }

        public DbSet<RentCaravan> RentCaravans { get; set; }

        public DbSet<RentCaravanPicture> RentCaravanPictures { get; set; }

        public DbSet<CaravanModule> CaravanModules { get; set; }
    }
}
