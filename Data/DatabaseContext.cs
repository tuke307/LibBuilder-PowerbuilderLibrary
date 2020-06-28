using Data.Models;
using Microsoft.EntityFrameworkCore;
using PBDotNetLib.orca;
using System;
using System.Linq;

namespace Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<SettingsModel> Settings { get; set; }
        public DbSet<WorkspaceModel> Workspace { get; set; }
        public DbSet<TargetModel> Target { get; set; }
        public DbSet<LibraryModel> Library { get; set; }
        public DbSet<ObjectModel> Object { get; set; }
        public DbSet<ProcessModel> Process { get; set; }

        public DatabaseContext()
        {
            Database.Migrate();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Filename={Constants.DatabasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Versions Speicherung
            modelBuilder.Entity<WorkspaceModel>()
                .Property(w => w.PBVersion)
                .HasConversion(
                    x => x.ToString(), // to converter
                    x => (Orca.Version)Enum.Parse(typeof(Orca.Version), x));// from converter

            //Type Speicherung
            modelBuilder.Entity<ObjectModel>()
                .Property(o => o.ObjectType)
                .HasConversion(
                    x => x.ToString(), // to converter
                    x => (Objecttype)Enum.Parse(typeof(Objecttype), x));// from converter

            //Seeding
            modelBuilder.Entity<SettingsModel>().HasData(
            new SettingsModel { Id = 1, DarkMode = false, PrimaryColor = "teal", SecondaryColor = "cyan" });
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                //Modified Date
                ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                //Creation Date
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}