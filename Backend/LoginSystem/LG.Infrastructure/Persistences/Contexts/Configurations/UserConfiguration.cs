using LG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LG.Infrastructure.Persistences.Contexts.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Users__1788CC4CFA1A2729");
            builder.Property(e => e.Id)
                .HasColumnName("UserId");
            builder.HasIndex(e => e.Mail, "UQ__Users__2724B2D1BE6086D4").IsUnique();
            builder.HasIndex(e => e.Username, "UQ__Users__536C85E4B890A1E0").IsUnique();

            builder.HasIndex(e => e.PersonId).IsUnique();

            builder.Property(e => e.AuditCreateDate).HasColumnType("datetime");
            builder.Property(e => e.AuditDeleteDate).HasColumnType("datetime");
            builder.Property(e => e.AuditUpdateDate).HasColumnType("datetime");
            builder.Property(e => e.FailedAttempts).HasDefaultValue(0);
            builder.Property(e => e.Mail)
                .HasMaxLength(120)
                .IsUnicode(false);
            builder.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.SessionActive)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("0")
                .IsFixedLength();
            builder.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("ACTIVO");
            builder.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false);
        }
    }
}
