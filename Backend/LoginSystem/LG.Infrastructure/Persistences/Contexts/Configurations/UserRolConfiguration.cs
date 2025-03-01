using LG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LG.Infrastructure.Persistences.Contexts.Configurations
{
    public class UserRolConfiguration : IEntityTypeConfiguration<UserRol>
    {
        public void Configure(EntityTypeBuilder<UserRol> builder)
        {
            builder.HasKey(e => new { e.RolId, e.UserId }).HasName("PK__User_rol__285B8E353CFD00A1");

            builder.ToTable("User_rol");

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

            builder.HasOne(d => d.Rol).WithMany(p => p.UserRols)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRol_Rol");

            builder.HasOne(d => d.User).WithMany(p => p.UserRols)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRol_Users");
        }
    }
}
