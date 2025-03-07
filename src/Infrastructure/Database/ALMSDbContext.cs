using Microsoft.EntityFrameworkCore;
using DomainDDD.Entities;
using DomainDDD.Entities.Item;
using DomainDDD.Entities.AssemblyProcess;
using DomainDDD.Entities.SubProduct;
using DomainDDD.Entities.Product;
using DomainDDD.Entities.Line;
using DomainDDD.Entities.OPU;
using DomainDDD.Entities.Station;
using DomainDDD.Entities.Detector;
// manual using so the compiler doesn't confuse it with System.Buffer
using Buffer = DomainDDD.Entities.Buffer.Buffer;

namespace Infrastructure.Database {
    public class ALMSDbContext : DbContext {
        public ALMSDbContext(DbContextOptions<ALMSDbContext> options) : base(options) {}

        // ALMS DB elements
        // TODO: figure out what data model we are actually using
        public DbSet<Item> Items => Set<Item>();
        public DbSet<AssemblyProcess> AssemblyProcesses => Set<AssemblyProcess>();
        public DbSet<SubProduct> SubProducts => Set<SubProduct>();
        public DbSet<Product> Products => Set<Product>();
        DbSet<Line> Lines => Set<Line>();
        DbSet<OPU> OPUs => Set<OPU>();
        DbSet<Station> Stations => Set<Station>();
        DbSet<Detector> Detectors => Set<Detector>();
        DbSet<Buffer> Buffers => Set<Buffer>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<AssemblyProcess>()
                .HasMany(proc => proc.items)
                .WithOne();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ALMSDbContext).Assembly);
        }
    }
}
