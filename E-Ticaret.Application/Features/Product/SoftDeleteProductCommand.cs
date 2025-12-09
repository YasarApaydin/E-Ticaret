using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Product;

   public sealed record SoftDeleteProductCommand(Guid Id):IRequest<Result<string>>;



internal sealed class SoftDeleteProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository) : IRequestHandler<SoftDeleteProductCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SoftDeleteProductCommand request, CancellationToken cancellationToken)
    {


        E_Ticaret.Domain.Entities.Product product = await productRepository.GetByExpressionAsync(c => c.Id == request.Id, cancellationToken);
        if (product is null)
        {
            return Result<string>.Failure("Ürün Bulunamadı.");

        }

        if (product.IsDeleted)
        {
            return Result<string>.Failure("Ürün zaten silinmiş.");
        }


        product.IsDeleted = true;
        product.DeletedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Ürün Başarıyla Silindi.");
    }


}

public sealed class SoftDeleteProductCommandValidator : AbstractValidator<SoftDeleteProductCommand>
{
    public SoftDeleteProductCommandValidator()
    {


        RuleFor(p => p.Id).NotEmpty().WithMessage("Ürün Id Boş Olamaz.");
        RuleFor(p => p.Id).NotNull().WithMessage("Ürün Id Boş Olamaz.");

    }
}

