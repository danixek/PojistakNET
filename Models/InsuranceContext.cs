// using PojistakNET.Migrations;
using Microsoft.EntityFrameworkCore;
using PojistakNET.Models;

namespace PojistakNET.Models
{
    public class InsuranceContext : DbContext
    {
        public InsuranceContext(DbContextOptions<InsuranceContext> options) : base(options)
        {

        }

        public DbSet<Insurer> Insurers { get; set; }
        public DbSet<Insurance> Insurances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Definování vztahu mezi Insurance a Insurer
            modelBuilder.Entity<Insurance>()
                .Property(i => i.Cost)
                .HasPrecision(10, 2); // Nastavení přesnosti pro 'Cost' (pojištění)

            modelBuilder.Entity<Insurance>()
                .HasOne(i => i.Insurer)        // Každé pojištění má jednoho pojištěnce
                .WithMany(i => i.Insurances)   // Pojištěnec může mít více pojištění
                .HasForeignKey(i => i.InsurerId) // Cizí klíč je InsurerId
                .OnDelete(DeleteBehavior.Cascade);  // (Volitelně) při smazání pojištěnce smaže i všechna pojištění
        }


    }
}
