using Microsoft.EntityFrameworkCore;
using Sims.API.Models;

namespace Sims.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Sim> Sims => Set<Sim>();
    public DbSet<SimDataSet> DataSets => Set<SimDataSet>();
}
