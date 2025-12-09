using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using FluentValidation.Validators;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Categories;
    public sealed record SoftDeleteCategoryCommand(Guid Id) : IRequest<Result<string>>;


internal sealed class SoftDeleteCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) : IRequestHandler<SoftDeleteCategoryCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SoftDeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = await categoryRepository.GetByExpressionAsync(c => c.Id == request.Id, cancellationToken);
        if (category is null)
        {
            return Result<string>.Failure("Category Bulunamadı.");

        }

        if (category.IsDeleted)
        {
            return Result<string>.Failure("Kategori zaten silinmiş.");
        }


        category.IsDeleted = true;
        category.DeletedAt = DateTime.UtcNow;
        category.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Kategori Başarıyla Silindi.");
    }
}


public class SoftDeleteCategoryCommandValidator : AbstractValidator<SoftDeleteCategoryCommand>
{
    public SoftDeleteCategoryCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("Category Id Boş Olamaz.");
        RuleFor(p => p.Id).NotNull().WithMessage("Category Id Boş Olamaz.");
    }

}