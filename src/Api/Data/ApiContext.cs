using Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
{
    public DbSet<Attraction> Attractions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attraction>().ToTable("Attractions");
    }
}