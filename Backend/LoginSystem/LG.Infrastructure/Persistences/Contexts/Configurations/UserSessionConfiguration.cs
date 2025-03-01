using LG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LG.Infrastructure.Persistences.Contexts.Configurations
{
    public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {
            builder.HasKey(e => e.SessionId).HasName("PK__User_ses__C9F492905D6295FF");

            builder.ToTable("User_sessions");

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
            builder.Property(e => e.CloseDate).HasColumnType("datetime");
            builder.Property(e => e.EntryDate).HasColumnType("datetime");

            builder.HasOne(d => d.User).WithMany(p => p.UserSessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSessions_Users");
        }
    }
}
