using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Product;
    public sealed  record RemoveProductCommand(Guid Id):IRequest<Result<string>>;


internal sealed class RemoveProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository) : IRequestHandler<RemoveProductCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {

        E_Ticaret.Domain.Entities.Product product = await productRepository.GetByExpressionAsync(p => p.Id == request.Id, cancellationToken);


        if (product is null)
        {
            return Result<string>.Failure("Ürün Bulunamadı.");
        }
        productRepository.Delete(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Ürün Başarıyla Veritabanından Silindi.");

    }
}


public sealed class RemoveProductCommandValidator : AbstractValidator<RemoveProductCommand>
{
    public RemoveProductCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("Ürün Id Boş Olamaz.");
        RuleFor(p => p.Id).NotNull().WithMessage("Ürün Id Boş Olamaz.");
    }
}