using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityTypeConfiguration
{
    public class WalletEntityTypeConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.WalletAddress).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Balance).IsRequired();
            entity.Property(e => e.Currency).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            // One-to-Many relationship
            entity.HasMany(e => e.Transactions)
                  .WithOne(e => e.Wallet)
                  .HasForeignKey(e => e.WalletId);
        }
    }
}
