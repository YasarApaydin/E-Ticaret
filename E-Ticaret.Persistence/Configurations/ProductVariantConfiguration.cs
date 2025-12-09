using E_Ticaret.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Ticaret.Persistence.Configurations;
internal sealed class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{


    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("ProductVariant");
        builder.HasKey(x => x.Id);
    }
}

