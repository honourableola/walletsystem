using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityTypeConfiguration
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder.HasKey(e => e.Id);
            modelBuilder.HasIndex(e => e.Email).IsUnique();
            modelBuilder.Property(e => e.Email).IsRequired().HasMaxLength(50);
            modelBuilder.Property(e => e.PasswordHash).IsRequired();
            modelBuilder.Property(e => e.RoleId).IsRequired();
            modelBuilder.Property(e => e.CreatedAt).IsRequired();

            // One-to-Many relationship
            modelBuilder.HasMany(e => e.Wallets)
                  .WithOne(e => e.User)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.HasMany(e => e.AuditLogs)
                  .WithOne(e => e.User)
                  .HasForeignKey(e => e.UserId);
        }
    }
}
