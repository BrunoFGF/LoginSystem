using LG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LG.Infrastructure.Persistences.Contexts.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Person__AA2FFBE5423AC441");
            builder.Property(e => e.Id)
                .HasColumnName("PersonId");

            builder.ToTable("Person");
            builder.HasIndex(e => e.IdentityCard, "UQ__Person__DA5B2F6DFC60A041").IsUnique();
            builder.Property(e => e.AuditCreateDate).HasColumnType("datetime");
            builder.Property(e => e.AuditDeleteDate).HasColumnType("datetime");
            builder.Property(e => e.AuditUpdateDate).HasColumnType("datetime");
            builder.Property(e => e.FirstName)
                .HasMaxLength(80)
                .IsUnicode(false);
            builder.Property(e => e.IdentityCard)
                .HasMaxLength(10)
                .IsUnicode(false);
            builder.Property(e => e.LastName)
                .HasMaxLength(80)
                .IsUnicode(false);
            builder.HasOne(p => p.User)
                .WithOne(u => u.Person)
                .HasForeignKey<User>(u => u.PersonId)
                .OnDelete(DeleteBehavior.Cascade) 
                .HasConstraintName("FK_Users_Person");
        }
    }
}
