using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sending.Domain.Players
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable("Players");

            builder
                .Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .HasKey(x => x.Id)
                .HasName("PK_Player_Id");

            builder
                .Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(32);

            builder
                .Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(56);

            builder
                .Property(x => x.Email)
                .HasMaxLength(128);

            builder
                .HasIndex(x => x.Email)
                .HasName("UX_Players_Email")
                .IsUnique();

            builder
                .Property(x => x.Birthday)
                .HasColumnType("date")
                .IsRequired();

            builder
                .Property(x => x.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasData(
                new Player
                {
                    Id = 1,
                    FirstName = "FirstName_Test1",
                    LastName = "LastName_Test1",
                    Email = "Email1@www.com",
                    Birthday = new DateTime(2010, 10, 10)
                },
                new Player
                {
                    Id = 2,
                    FirstName = "FirstName_Test2",
                    LastName = "LastName_Test2",
                    Email = "Email2@www.com",
                    Birthday = new DateTime(2000, 10, 10)
                },
                new Player
                {
                    Id = 3,
                    FirstName = "FirstName_Test3",
                    LastName = "LastName_Test3",
                    Email = "Email3@www.com",
                    Birthday = new DateTime(1990, 10, 10)
                });
        }
    }
}
