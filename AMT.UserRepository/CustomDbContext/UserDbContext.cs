using AMT.UserRepository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AMT.UserRepository.CustomDbContext
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User model-table relationship
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>()                
                .HasMany(e=>e.ChatMessagesToUsers)
                .WithOne(e=>e.FromUser)
                .HasForeignKey(e=>e.FromUserId)
                .HasConstraintName("FK_ChatMessage_From_User");
            modelBuilder.Entity<User>()
                .HasMany(e=>e.ChatMessagesFromUsers)
                .WithOne(e=>e.ToUser)
                .HasForeignKey(e=>e.ToUserId)
                .HasConstraintName("FK_ChatMessage_To_User");
            modelBuilder.Entity<User>()
                .HasMany(e=>e.Passwords)
                .WithOne(e=>e.User)
                .HasForeignKey(e=>e.UserId)
                .HasConstraintName("FK_Password_User");

            //ChatMessage model-table relationship
            modelBuilder.Entity<ChatMessage>().ToTable("ChatMessage");
            
            //Password model-table relationship
            modelBuilder.Entity<Password>().ToTable("Password");
            modelBuilder.Entity<Password>()
                .HasOne(e=>e.HashAlgorithm)
                .WithMany(e=>e.Passwords)
                .HasForeignKey(e=>e.HashAlgorithmId)
                .HasConstraintName("FK_Password_HashingAlgorithm");

            //HashingAlgorithm model-table relationship
            modelBuilder.Entity<HashingAlgorithm>().ToTable("HashingAlgorithm");
        }
    }
}
