using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Categories;
public sealed record CreateCategoryCommand(
   string Name,
   string Description
   ) : IRequest<Result<string>>;

internal sealed class CreateCategoryCommandHandler(ICategoryRepository categoryRepository,IUnitOfWork unitOfWork) : IRequestHandler<CreateCategoryCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var isCheck = await categoryRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);

        if (isCheck)
        {
            return Result<string>.Failure("Bu Kategori daha önce oluşturuldu.");
            

        }


        Category category = request.Adapt<Category>();

        categoryRepository.Add(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Kategori Başarıyla Oluşturuldu.");




    }
}
public  class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(p => p.Name).NotNull().WithMessage("Category Adnı Boş Olamaz.");
        RuleFor(p => p.Name).NotEmpty().WithMessage("Category Adnı Boş Olamaz.");
        RuleFor(p => p.Name).MinimumLength(3).WithMessage("Category Adı 3 karakterden Az Olamaz.");

        RuleFor(p => p.Description).NotNull().WithMessage("Category Acıklaması Boş Olamaz.");
        RuleFor(p => p.Description).NotEmpty().WithMessage("Category Acıklaması Boş Olamaz.");
        RuleFor(p => p.Description).MinimumLength(5).WithMessage("Category Acıklaması 5 karakterden Az Olamaz.");
    }

}