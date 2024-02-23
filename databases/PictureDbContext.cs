using Microsoft.EntityFrameworkCore;
using Task_6.Models;

namespace Task_6.dabases
{
    public class PictureDbContext : DbContext
    {
        public PictureDbContext(DbContextOptions<PictureDbContext> options) : base(options) { }

        public DbSet<PictureModel> Pictures { get; set; }
    }
}
