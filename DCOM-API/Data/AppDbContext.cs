using DCOM_API.Entities;
using DCOM_API.Services;
using Microsoft.EntityFrameworkCore;

namespace DCOM_API.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUser;

        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUser)
            : base(options)
        {
            _currentUser = currentUser;
        }

        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Study> Studies => Set<Study>();
        public DbSet<Series> Series => Set<Series>();
        public DbSet<DicomFile> DicomFiles => Set<DicomFile>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // --- Multi-tenant: her sorguya otomatik "sadece benim verim" filtresi ---
            modelBuilder.Entity<Patient>().HasQueryFilter(p => p.UserId == _currentUser.UserId);
            modelBuilder.Entity<Study>().HasQueryFilter(s => s.UserId == _currentUser.UserId);
            modelBuilder.Entity<Series>().HasQueryFilter(s => s.UserId == _currentUser.UserId);
            modelBuilder.Entity<DicomFile>().HasQueryFilter(d => d.UserId == _currentUser.UserId);

            // --- Benzersizlik artık KULLANICI BAZINDA (aynı hasta farklı kullanıcılarda olabilir) ---
            modelBuilder.Entity<Patient>()
                .HasIndex(p => new { p.UserId, p.PatientId })
                .IsUnique();

            modelBuilder.Entity<Study>()
                .HasIndex(s => new { s.UserId, s.StudyInstanceUid })
                .IsUnique();

            modelBuilder.Entity<Series>()
                .HasIndex(s => new { s.UserId, s.SeriesInstanceUid })
                .IsUnique();

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Studies)
                .WithOne(s => s.Patient)
                .HasForeignKey(s => s.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Study>()
                .HasMany(s => s.Series)
                .WithOne(se => se.Study)
                .HasForeignKey(se => se.StudyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Series>()
                .HasMany(se => se.DicomFiles)
                .WithOne(df => df.Series)
                .HasForeignKey(df => df.SeriesId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        // --- Kaydederken yeni eklenen her kayda otomatik UserId yaz ---
        public override int SaveChanges()
        {
            ApplyUserId();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyUserId();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyUserId()
        {
            var userId = _currentUser.UserId;
            if (userId is null) return;

            foreach (var entry in ChangeTracker.Entries<IOwnedEntity>())
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.UserId = userId.Value;
            }
        }
    }
}