using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PojistakNET.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<LogEntry> LogEntries { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // <<< Tohle je nutné!

        modelBuilder.Entity<LogEntry>().ToTable("LogEntries"); // Zajistí, že se nebude hledat tabulka "LogEntry"
    }
    public DbSet<Article> Articles { get; set; }
}