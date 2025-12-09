using E_Ticaret.Domain.Interfaces.Repositories;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;


namespace E_Ticaret.Application.Features.Product;
    public sealed record CreateProductCommand(string Name,
        string Description,
        decimal Price,
        int Stoct,
        string ImageUrl,
        Guid CategoryId):IRequest<Result<string>>;



internal sealed class CreateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository) : IRequestHandler<CreateProductCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {


        var isCheck = await productRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);

        if (isCheck)
        {
            return Result<string>.Failure("Bu Ürün Zaten Mevcut.");
        }

        E_Ticaret.Domain.Entities.Product product = request.Adapt<E_Ticaret.Domain.Entities.Product>();
        productRepository.Add(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed("Ürün Başarıyla Oluşturuldu.");
    }
}
   


public sealed class CreateProductCommandValidator: AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Ürün Adı Boş Olamaz.");
        RuleFor(p => p.Name).NotNull().WithMessage("Ürün Adı Boş Olamaz.");
        RuleFor(p => p.Name).MinimumLength(3).WithMessage("Ürün Adı En az 3 Karakter Olmalıdır.");
        RuleFor(p => p.Price).NotEmpty().WithMessage("Ürün Fiyatı Boş Olamaz.");
        RuleFor(p => p.Price).NotNull().WithMessage("Ürün Fiyatı Boş Olamaz.");

        RuleFor(p => p.CategoryId).NotEmpty().WithMessage("Category Boş Olamaz.");
        RuleFor(p => p.CategoryId).NotNull().WithMessage("Category Boş Olamaz.");

        RuleFor(p => p.Price).GreaterThan(0).WithMessage("Ürün Fiyatı 0 Olamaz.");

        RuleFor(p => p.Stoct).NotEmpty().WithMessage("Ürün Stock Boş Olamaz.");
        RuleFor(p => p.Stoct).NotNull().WithMessage("Ürün Stock Boş Olamaz.");
        RuleFor(p => p.Stoct).GreaterThan(0).WithMessage("Ürün Miktarı 0 Olamaz.");


        RuleFor(p => p.Description).NotEmpty().WithMessage("Ürün Acıklaması Boş Olamaz.");
        RuleFor(p => p.Description).NotNull().WithMessage("Ürün Acıklaması Boş Olamaz.");
        RuleFor(p => p.Description).MinimumLength(5).WithMessage("Ürün Acıklaması 5 Karakterden Az Olamaz.");
    }
}