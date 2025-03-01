using LG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LG.Infrastructure.Persistences.Contexts.Configurations
{
    public class RolOptionConfiguration : IEntityTypeConfiguration<RolOption>
    {
        public void Configure(EntityTypeBuilder<RolOption> builder)
        {
            builder.HasKey(e => e.OptionId).HasName("PK__RolOptio__92C7A1FF0BF571BB");

            builder.Property(e => e.AuditCreateDate).HasColumnType("datetime");
            builder.Property(e => e.AuditCreateUser)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.AuditDeleteDate).HasColumnType("datetime");
            builder.Property(e => e.AuditDeleteUser)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.AuditUpdateDate).HasColumnType("datetime");
            builder.Property(e => e.AuditUpdateUser)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.OptionName)
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
