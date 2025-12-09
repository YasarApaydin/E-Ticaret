using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace E_Ticaret.Application.Features.Product;
    public sealed record UpdateProductCommand(
          Guid Id,
        string Name,
        string Description,
        decimal Price,
        int Stoct,
        string ImageUrl,
        Guid CategoryId
        ) : IRequest<Result<string>>;


internal sealed class UpdateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository) : IRequestHandler<UpdateProductCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        E_Ticaret.Domain.Entities.Product product = await productRepository.GetByExpressionAsync(p => p.Id == request.Id, cancellationToken);

        if(product is null)
        {
            return Result<string>.Failure("Ürün Bulunamadı.");
        }


        var isChek = await productRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);

        if (isChek)
        {
            return Result<string>.Failure("Bu Ürün Daha Önce Oluşturuldu.");
        }


        request.Adapt(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Ürün Başarılı Bir Şekilde Güncellendi.");

    }
}
    


public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Ürün Adı Boş Olamaz.");
        RuleFor(p => p.Name).NotNull().WithMessage("Ürün Adı Boş Olamaz.");
        RuleFor(p => p.Name).MinimumLength(3).WithMessage("Ürün Adı En az 3 Karakter Olmalıdır.");
        RuleFor(p => p.Price).NotEmpty().WithMessage("Ürün Fiyatı Boş Olamaz.");
        RuleFor(p => p.Price).NotNull().WithMessage("Ürün Fiyatı Boş Olamaz.");

        RuleFor(p => p.Id).NotEmpty().WithMessage("Ürün Id Boş Olamaz.");
        RuleFor(p => p.Id).NotNull().WithMessage("Ürün Id Boş Olamaz.");



        RuleFor(p => p.Stoct).NotEmpty().WithMessage("Ürün Stock Boş Olamaz.");
        RuleFor(p => p.Stoct).NotNull().WithMessage("Ürün Stock Boş Olamaz.");


        RuleFor(p => p.Description).NotEmpty().WithMessage("Ürün Acıklaması Boş Olamaz.");
        RuleFor(p => p.Description).NotNull().WithMessage("Ürün Acıklaması Boş Olamaz.");

        RuleFor(p => p.Description).MinimumLength(5).WithMessage("Ürün Acıklaması 5 Karakterden Az Olamaz.");
    }
}