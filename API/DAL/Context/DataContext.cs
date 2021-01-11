using Marcplaats_Backend.BLL.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.DAL.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(pr => new { pr.UserID, pr.RoleID });
            modelBuilder.Entity<UserRole>()
                .HasOne(pr => pr.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(pr => pr.UserID);
            modelBuilder.Entity<UserRole>()
                .HasOne(pr => pr.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(pr => pr.RoleID);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Advertisements)
                .WithOne(a => a.Category);

            modelBuilder.Entity<ChatUser>()
                .HasKey(cu => new { cu.UserID, cu.ChatID });
            modelBuilder.Entity<ChatUser>()
                .HasOne(ch => ch.Chat)
                .WithMany(cu => cu.ChatUsers)
                .HasForeignKey(ch => ch.ChatID);
            modelBuilder.Entity<ChatUser>()
                .HasOne(p => p.User)
                .WithMany(cu => cu.ChatUsers)
                .HasForeignKey(ch => ch.UserID);

            modelBuilder.Entity<Chat>()
                .HasMany(msg => msg.Messages)
                .WithOne(ch => ch.Chat);
        }
    }
}
