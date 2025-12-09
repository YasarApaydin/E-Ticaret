using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Events.Users;
using E_Ticaret.Domain.Interfaces.Repositories;
using E_Ticaret.Infrastructure.Abstractions;
using FluentValidation;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace E_Ticaret.Application.Features.Auth;
    public sealed record RegisterCommand(
        string FirstName,
        string LastName,
        string Email,
        string Phone,
        string Password,
        string UserName,
        string ImageUrl


        ) :IRequest<Result<string>>;


internal sealed class RegisterCommandHandler(UserManager<AppUser> userManager, IImageRepository ımageRepository, IUnitOfWork unitOfWork, IMediator mediator, IEncryptionService encryptionService) : IRequestHandler<RegisterCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var checkIsUserName = await userManager.FindByNameAsync(request.UserName);
        if (checkIsUserName is not null)
        {
           return Result<string>.Failure("Bu Kullanıcı Adı Kayıtlı.");

        }

        var checkEmailExists = await userManager.FindByEmailAsync(request.Email);

        if (checkEmailExists is not null)
        {
            return Result<string>.Failure("Bu Email Kayıtlı");
        }

        var checkPhone = await userManager.Users.AnyAsync(u => u.PhoneNumber == request.Phone);

        if (checkPhone)
        {
            return Result<string>.Failure("Bu Telefon Numarası Kayıtlıdır.");

        }

        string encryptedImageUrl = encryptionService.EncryptString(request.ImageUrl);



        Image ımage = new()
        {
            ImageUrl = encryptedImageUrl
        };
        await ımageRepository.AddAsync(ımage, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);



        //   string encrytedEmail = encryptionService.EncryptString(request.Email);
        string encrytedFirstName = encryptionService.EncryptString(request.FirstName);
        string encrytedLastName = encryptionService.EncryptString(request.LastName);
        //     string encrytedUserName = encryptionService.EncryptString(request.UserName);
        //   string encrytedPhone = encryptionService.EncryptString(request.Phone);





        AppUser appUser = new()
        {
            Id = Guid.CreateVersion7(),
            Email = request.Email,
            FirstName = encrytedFirstName,
            LastName = encrytedLastName,
            UserName = request.UserName,
            PhoneNumber = request.Phone,
            ImageId = ımage.Id


        };



        var user = await userManager.CreateAsync(appUser, request.Password);
        if (!user.Succeeded)
        {
            return Result<string>.Failure("Kullanıcı Oluşturulamadı.");
        }
        var verifitionCode = new Random().Next(100000, 999999).ToString();



        await mediator.Publish(new UserDomainEvent(appUser, verifitionCode), cancellationToken);

        return Result<string>.Succeed("Kullanıcı Başarıyla Oluşturuldu.");

    }
}

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{


    public RegisterCommandValidator()
    {

        RuleFor(p => p.UserName).NotEmpty().WithMessage("Kullanıcı Adı Boş Olamaz.");
        RuleFor(p => p.UserName).NotNull().WithMessage("Kullanıcı Adı Boş Olamaz");
        RuleFor(p => p.UserName).MinimumLength(3).WithMessage("Kullanıcı En Az 3 Karakter Olmalıdır.");
        RuleFor(p => p.FirstName).NotNull().WithMessage("Ad Boş Olamaz.");
        RuleFor(p => p.FirstName).NotEmpty().WithMessage("Ad Boş Olamaz.");
        RuleFor(p => p.FirstName).MinimumLength(3).WithMessage("Ad 3 Karakterden Az Olamaz.");
        RuleFor(p => p.LastName).NotNull().WithMessage("Soyad Boş Olamaz.");
        RuleFor(p => p.LastName).NotEmpty().WithMessage("Soyad Boş Olamaz.");
        RuleFor(p => p.LastName).MinimumLength(3).WithMessage("Soyad 3 Karakterden Az Olamaz.");
        RuleFor(p => p.Email).NotEmpty().WithMessage("Email Boş Olamaz");
        RuleFor(p => p.Email).NotNull().WithMessage("Email Boş Olamaz");
        RuleFor(p => p.Email).MinimumLength(3).WithMessage("Mail Adresi En Az 3 Karakter Olamlıdır.");
        RuleFor(p => p.Email).EmailAddress().WithMessage("Gecerli Bir Mail Adresi Girin.");
        RuleFor(p => p.Password).NotNull().WithMessage("Şifre Boş Olamaz");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Şifre Boş Olamaz");
        RuleFor(p => p.Password).MinimumLength(8).WithMessage("Şifre En Az 8 Karakter Olmalıdır.");
        RuleFor(p => p.Password).Matches("[A-Z]").WithMessage("Şifre En Az 1 Büyük Harf İçermelidir.");
        RuleFor(p => p.Password).Matches("[a-z]").WithMessage("Şifre En Az 1 Küçük Harf İçermelidir.");
        RuleFor(p => p.Password).Matches("[^a-zA-z0-9]").WithMessage("Şifre En Az 1 Adet Özel Karakter İçermelidir.");

        RuleFor(p => p.Phone).NotEmpty().WithMessage("Kullanıcı Telefon Numarası Boş Olamaz.");
        RuleFor(p => p.Phone).NotNull().WithMessage("Kullanıcı Telefon Numarası Boş Olamaz");
        RuleFor(p => p.Phone).MinimumLength(11).WithMessage("Kullanıcı Telefon Numarası En Az 11 Karakter Olmalıdır.");
        RuleFor(p => p.Phone).Matches(@"^[0-9]+$").WithMessage("Kullanıcı Telefon Numarası Sadece Rakamlardan Oluşmalıdır.");
        RuleFor(p => p.Phone).MaximumLength(15).WithMessage("Kullanıcı Telefon Numarası En Fazla 15 Karakterden Oluşmalıdır.");




        RuleFor(p => p.ImageUrl).NotEmpty().WithMessage("Kullanıcı Resmi Boş Olamaz.");
        RuleFor(p => p.ImageUrl).NotNull().WithMessage("Kullanıcı Resmi Boş Olamaz");
    }
}