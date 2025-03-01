using LG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LG.Infrastructure.Persistences.Contexts.Configurations
{
    public class RolRolOptionConfiguration : IEntityTypeConfiguration<RolRolOption>
    {
        public void Configure(EntityTypeBuilder<RolRolOption> builder)
        {
            builder.HasKey(e => new { e.RolId, e.OptionId }).HasName("PK__Rol_rolO__100F78EE4B42416D");

            builder.ToTable("Rol_rolOptions");

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

            builder.HasOne(d => d.Option).WithMany(p => p.RolRolOptions)
                .HasForeignKey(d => d.OptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolRolOptions_RolOptions");

            builder.HasOne(d => d.Rol).WithMany(p => p.RolRolOptions)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolRolOptions_Rol");
        }
    }
}
