using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("BOOK");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasColumnType("INT");;

        builder.Property(x => x.Title)
            .IsRequired()
            .HasColumnType("NVARCHAR(250)")
            .IsUnicode();
        
        builder.Property(x => x.CoverUri)
            .HasColumnType("VARCHAR(70)");

        builder.Property(x => x.Subject)
            .IsRequired()
            .HasColumnType("VARCHAR(30)")
            .HasConversion(x => x.ToString(), x => Enum.Parse<Subject>(x));

        builder.Property(x => x.PublishYear)
            .IsRequired()
            .HasColumnType("INT");

        builder.HasOne(x => x.Author)
            .WithMany(x => x.Books)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
