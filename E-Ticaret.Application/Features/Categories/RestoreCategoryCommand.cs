using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Categories;
    public sealed record RestoreCategoryCommand(Guid Id):IRequest<Result<string>>;


internal sealed class RestoreCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) : IRequestHandler<RestoreCategoryCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RestoreCategoryCommand request, CancellationToken cancellationToken)
    {

        Category category = await categoryRepository.GetByExpressionAsync(c => c.Id == request.Id, cancellationToken);

        if(category is null)
        {
            return Result<string>.Failure("Kategori Bulunamadı.");
        }

        if (!category.IsDeleted)
        {
            return Result<string>.Failure("Kategori Zaten Aktif Durumda.");
        }


        category.IsDeleted = false;
        category.DeletedAt = null;
        category.UpdatedAt = DateTime.UtcNow;


        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Kategori Başarıyla Geri Yüklendi.");



    }
}
 

public sealed class RestoreCategoryCommandValidator : AbstractValidator<RestoreCategoryCommand>
{
    public RestoreCategoryCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("Ürün Id Boş Olamaz.");
        RuleFor(p => p.Id).NotNull().WithMessage("Ürün Id Boş Olamaz.");
    }
}