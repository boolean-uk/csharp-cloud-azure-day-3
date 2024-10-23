using backend.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace backend.wwwapi.DataContext
{
    public class CatContext : DbContext
    {
        public CatContext(DbContextOptions<CatContext> options) : base(options) { }

        public DbSet<Cat> Cats { get; set; }
    }
}
