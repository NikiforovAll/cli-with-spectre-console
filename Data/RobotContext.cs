namespace CliWithSpectreConsole.Data;

using Microsoft.EntityFrameworkCore;

public class RobotContext : DbContext
{
    public RobotContext(DbContextOptions<RobotContext> options)
        : base(options)
    {
    }

    public DbSet<Robot> Robots => this.Set<Robot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Robot>().HasKey(r => r.Uri);

        base.OnModelCreating(modelBuilder);
    }
}
