using backend.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace backend.wwwapi.DataContext
{
    public class DogContext : DbContext
    {
        private static string GetConnectionString()
        {
            string jsonSettings = File.ReadAllText("appsettings.json");
            JObject configuration = JObject.Parse(jsonSettings);
            return configuration["ConnectionStrings"]["DefaultConnection"].ToString();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(GetConnectionString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dog>().HasData(
                  new Dog { Id = 1, Name = "First task", Completed = true },
                  new Dog { Id = 2, Name = "Second task", Completed = false },
                  new Dog { Id = 3, Name = "Third task", Completed = true }
              );
        }

        public DbSet<Dog> Dogs { get; set; }
    }
}