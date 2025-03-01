using LG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LG.Infrastructure.Persistences.Contexts.Configurations
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Rol__F92302F1B880FCA0");
            builder.Property(e => e.Id)
                .HasColumnName("RolId");

            builder.ToTable("Rol");

            builder.Property(e => e.AuditCreateDate).HasColumnType("datetime");
            builder.Property(e => e.AuditDeleteDate).HasColumnType("datetime");
            builder.Property(e => e.AuditUpdateDate).HasColumnType("datetime");
            builder.Property(e => e.RolName)
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
