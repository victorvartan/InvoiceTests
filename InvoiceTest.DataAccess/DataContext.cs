using InvoiceTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceTest.DataAccess
{
    public class DataContext : DbContext
    {
        public Guid CurrentUserId { get; set; }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceNote> InvoiceNotes { get; set; }

        public DbSet<User> Users { get; set; }

        public DataContext() : base()
        {
        }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>().HasIndex(i => i.Identifier).IsUnique();
            modelBuilder.Entity<User>().HasIndex(i => i.ApiKey).IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            SetInfoOnSave();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetInfoOnSave();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            SetInfoOnSave();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            SetInfoOnSave();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetInfoOnSave()
        {
            var dbEntityEntries = ChangeTracker.Entries();

            foreach (var entry in dbEntityEntries)
            {
                var entity = entry.Entity as BaseEntity;

                if (entity != null)
                {
                    var currentDate = DateTime.UtcNow;
                    if (entry.State == EntityState.Added)
                    {
                        entity.DateCreated = currentDate;
                        entity.CreatedByUserId = CurrentUserId;
                    }
                    else
                    {
                        entity.DateLastUpdated = currentDate;
                        entity.LastUpdatedByUserId = CurrentUserId;
                    }
                }
            }
        }
    }
}
