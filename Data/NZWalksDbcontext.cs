using Microsoft.EntityFrameworkCore;
using NZWalks.Models.Domain;

namespace NZWalks.Data
{
    public class NZWalksDbcontext : DbContext
    {
        public NZWalksDbcontext(DbContextOptions<NZWalksDbcontext> options) : base(options)
        {
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
    }
}
