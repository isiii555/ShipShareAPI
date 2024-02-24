using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipShareAPI.Domain.Entities;
using ShipShareAPI.Domain.Entities.Common;

namespace ShipShareAPI.Persistence.Context
{
    public class ShipShareDbContext : DbContext
    {
        public ShipShareDbContext(DbContextOptions options) : base(options) { }
        public DbSet<SenderPost> SenderPosts { get; set; }
        public DbSet<TravellerPost> TravellerPosts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.TravellerPosts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.SenderPosts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            BeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public void BeforeSaving()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                ((BaseEntity)item.Entity).LastModifiedDate = DateTime.Now;
                if (item.State == EntityState.Added)
                {
                    ((BaseEntity)item.Entity).CreatedDate = DateTime.Now;
                }
            }
        }
    }
}
