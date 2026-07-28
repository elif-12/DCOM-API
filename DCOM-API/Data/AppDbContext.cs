using DCOM_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DCOM_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
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

            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.PatientId)
                .IsUnique();

            modelBuilder.Entity<Study>()
                .HasIndex(s => s.StudyInstanceUid)
                .IsUnique();

            modelBuilder.Entity<Series>()
                .HasIndex(s => s.SeriesInstanceUid)
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
    }
}
