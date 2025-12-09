using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Categories;
    public sealed record RemoveCategoryCommand(Guid Id): IRequest<Result<string>>;



internal sealed class RemoveCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) : IRequestHandler<RemoveCategoryCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = await categoryRepository.GetByExpressionAsync(c => c.Id == request.Id, cancellationToken);
        if(category is null)
        {
            return Result<string>.Failure("Category Bulunamadı.");

        }

        categoryRepository.Delete(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Category Başarıyla Silindi.");


    }
}


public class RemoveCategoryCommandValidator: AbstractValidator<RemoveCategoryCommand>
{
    public RemoveCategoryCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("Category Id Boş Olamaz.");
        RuleFor(p => p.Id).NotNull().WithMessage("Category Id Boş Olamaz.");
    }
}