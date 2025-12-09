using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Product;


    public sealed record  RestoreProductCommand(Guid Id):IRequest<Result<string>>;





internal sealed class RestoreProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository) : IRequestHandler<RestoreProductCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RestoreProductCommand request, CancellationToken cancellationToken)
    {

        E_Ticaret.Domain.Entities.Product product = await productRepository.GetByExpressionAsync(c => c.Id == request.Id, cancellationToken);

        if (product is null)
        {
            return Result<string>.Failure("Ürün Bulunamadı.");
        }

        if (!product.IsDeleted)
        {
            return Result<string>.Failure("Ürün Zaten Aktif Durumda.");
        }


        product.IsDeleted = false;
        product.DeletedAt = null;
        product.UpdatedAt = DateTime.UtcNow;


        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Ürün Başarıyla Geri Yüklendi.");



    }
}



public sealed class RestoreProductCommandValidator : AbstractValidator<RestoreProductCommand>
{
    public RestoreProductCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("Ürün Id Boş Olamaz.");
        RuleFor(p => p.Id).NotNull().WithMessage("Ürün Id Boş Olamaz.");
    }
}