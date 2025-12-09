using E_Ticaret.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret.Persistence.Configurations
{
    internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p => p.Name).HasColumnType("varchar(MAX)");
            builder.Property(p => p.Description).HasColumnType("varchar(MAX)");
            builder.ToTable("Categories");
            builder.HasKey(x => x.Id);
        }
    }
}
