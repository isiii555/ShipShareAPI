﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleUser> RoleUser { get; set; }
        public DbSet<ConversationUser> ConversationUser { get; set; }
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

            modelBuilder.Entity<User>()
                .HasMany(i => i.Roles)
                .WithMany(u => u.Users)
                .UsingEntity<RoleUser>();

            modelBuilder.Entity<Review>()
                .HasOne(r => r.ReviewSender)
                .WithMany(u => u.GivenReviews)
                .HasForeignKey(r => r.ReviewSenderId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.ReviewRecipient)
                .WithMany(u => u.ReceivedReviews)
                .HasForeignKey(r => r.ReviewRecipientId);


            modelBuilder.Entity<User>()
                .HasMany(i => i.Conversations)
                .WithMany(u => u.Users)
                .UsingEntity<ConversationUser>();

            modelBuilder.Entity<Conversation>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Conversation)
                .HasForeignKey(m => m.ConversationId);


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
                if (item.Entity is BaseEntity entity)
                {
                    entity.LastModifiedDate = DateTime.Now;
                    if (item.State == EntityState.Added)
                    {
                        entity.CreatedDate = DateTime.Now;
                    }
                }
            }
        }
    }
}
