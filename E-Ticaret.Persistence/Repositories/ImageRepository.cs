using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using E_Ticaret.Persistence.Context;
using GenericRepository;

namespace E_Ticaret.Persistence.Repositories;
internal sealed class ImageRepository : Repository<Image, AppDbContext>, IImageRepository
{
    public ImageRepository(AppDbContext context) : base(context)
    {
    }
}

