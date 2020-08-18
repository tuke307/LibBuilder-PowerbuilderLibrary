// project=Data, file=DatabaseContext.cs, create=20:12 Copyright (c) 2020 tuke
// productions. All rights reserved.
using Data.Models;
using Microsoft.EntityFrameworkCore;
using PBDotNetLib.orca;
using System;
using System.Linq;

namespace Data
{
    /// <summary>
    /// DatabaseContext.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        /// <value>The library.</value>
        public DbSet<LibraryModel> Library { get; set; }

        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>The object.</value>
        public DbSet<ObjectModel> Object { get; set; }

        /// <summary>
        /// Gets or sets the process.
        /// </summary>
        /// <value>The process.</value>
        public DbSet<ProcessModel> Process { get; set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>The settings.</value>
        public DbSet<SettingsModel> Settings { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public DbSet<TargetModel> Target { get; set; }

        /// <summary>
        /// Gets or sets the workspace.
        /// </summary>
        /// <value>The workspace.</value>
        public DbSet<WorkspaceModel> Workspace { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseContext" /> class.
        /// </summary>
        public DatabaseContext()
        {
            // migrate and create
            Database.Migrate();
        }

        /// <summary>
        /// <para>Saves all changes made in this context to the database.</para>
        /// <para>
        /// This method will automatically call <see
        /// cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges"
        /// /> to discover any changes to entity instances before saving to the underlying
        /// database. This can be disabled via <see
        /// cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled"
        /// />.
        /// </para>
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
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

        /// <summary>
        /// Called when [configuring].
        /// </summary>
        /// <param name="options">The options.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Filename={Constants.DatabasePath}");
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by
        /// convention from the entity types exposed in <see
        /// cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived
        /// context. The resulting model may be cached and re-used for subsequent
        /// instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">
        /// The builder being used to construct the model for this context. Databases (and
        /// other extensions) typically define extension methods on this object that allow
        /// you to configure aspects of the model that are specific to a given database.
        /// </param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see
        /// cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)"
        /// />) then this method will not be run.
        /// </remarks>
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
    }
}