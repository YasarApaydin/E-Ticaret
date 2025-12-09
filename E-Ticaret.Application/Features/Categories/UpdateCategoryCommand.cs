using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Categories;
    public sealed record UpdateCategoryCommand(
        Guid Id,
        string Name,
        string Description
        ) : IRequest<Result<string>>;


internal sealed class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCategoryCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = await categoryRepository.GetByExpressionAsync(p => p.Id == request.Id,cancellationToken);

        if(category is null)
        {
            return Result<string>.Failure("Category Bulunamadı.");
        }


        if(category.Name != request.Name)
        {
            var isCheck = await categoryRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);

            if (isCheck)
            {
                return Result<string>.Failure("Aynı İsimde Kategori Mevcuttur.");
            }



            request.Adapt(category);
            await unitOfWork.SaveChangesAsync(cancellationToken);


        }
        return Result<string>.Failure("Kategori başarıyla güncellendi.");




    }
}


public  class UpdateCategoryValidator: AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(p => p.Id).NotNull().WithMessage("Category Id Boş Olamaz.");
        RuleFor(p => p.Id).NotEmpty().WithMessage("Category Id Boş Olamaz.");
        RuleFor(p => p.Name).NotNull().WithMessage("Category Adı Boş Olamaz.");
        RuleFor(p => p.Name).NotEmpty().WithMessage("Category Adı Boş Olamaz.");
        RuleFor(p => p.Name).MinimumLength(3).WithMessage("Category Adı 3 karakterden Az Olamaz.");



        RuleFor(p => p.Description).NotNull().WithMessage("Category Acıklaması Boş Olamaz.");
        RuleFor(p => p.Description).NotEmpty().WithMessage("Category Acıklaması Boş Olamaz.");
        RuleFor(p => p.Description).MinimumLength(5).WithMessage("Category Acıklaması 5 karakterden Az Olamaz.");
    }
}