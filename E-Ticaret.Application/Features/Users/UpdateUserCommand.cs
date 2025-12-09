using E_Ticaret.Domain.Entities;
using E_Ticaret.Domain.Interfaces.Repositories;
using E_Ticaret.Infrastructure.Abstractions;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace E_Ticaret.Application.Features.Users;
    public sealed record UpdateUserCommand(
       Guid UserId,
    string? FirstName,
    string? LastName,
    string? UserName,
    string? Email,
    string? Phone,
    string? ImageUrl,
    string? CurrentPassword,
    string? NewPassword,
    string? ConfirmNewPassword
        ) :IRequest<Result<string>>;


internal sealed class UpdateUserCommandHandler(UserManager<AppUser> userManager, IImageRepository imageRepository, IEncryptionService encryptionService) : IRequestHandler<UpdateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {


        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı Bulunamadı.");
        }

        bool isUpdate = false;

        if (!string.IsNullOrWhiteSpace(request.FirstName))
        {
            user.FirstName = encryptionService.EncryptString(request.FirstName);
            isUpdate = true;
        }

        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            user.LastName = encryptionService.EncryptString(request.LastName);
            isUpdate = true;
        }
        if (!string.IsNullOrWhiteSpace(request.UserName))
        {
            user.UserName = encryptionService.EncryptString(request.UserName);
            isUpdate = true;
        }
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            user.Email = encryptionService.EncryptString(request.Email);
            isUpdate = true;
        }


        if (!string.IsNullOrWhiteSpace(request.Phone))
        {
            user.PhoneNumber = request.Phone;
            isUpdate = true;
        }



        if (!string.IsNullOrEmpty(request.NewPassword))
        {
            if (!string.IsNullOrEmpty(request.CurrentPassword))
            {
                return Result<string>.Failure("Mevcut Şifreyi Girmek Zorundasınız.");
            }
            if(request.NewPassword != request.ConfirmNewPassword)
            {
                return Result<string>.Failure("Yeni şifre ile doğrulama şifresi eşleşmiyor.");
            }

            var check = await userManager.CheckPasswordAsync(user, request.CurrentPassword!);

            if (!check)
            {
                return Result<string>.Failure("Mevcut Şifre Zorunludur.");
            }

            var password = await userManager.ChangePasswordAsync(user,request.CurrentPassword!,request.NewPassword);
            if(!password.Succeeded)
            {
                return Result<string>.Failure("Şifre Güncellenemedi.");
            }

            isUpdate = true;
        }




        if (!string.IsNullOrWhiteSpace(request.Phone))
        {
            string encryptedImage = encryptionService.EncryptString(request.ImageUrl!);

            var existImage = await imageRepository.Where(x => x.Id == user.ImageId).FirstOrDefaultAsync(cancellationToken);
            if (existImage is not null)
            {
                existImage.ImageUrl = encryptedImage;
                imageRepository.Update(existImage);
                isUpdate = true;
            }



        }




        if (isUpdate)
        {
            await userManager.UpdateAsync(user);
        }
        return Result<string>.Succeed("Profil başarıyla güncellendi.");


    }
}



public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(p => p.UserId).NotEmpty().WithMessage("Kullanıcı Id Boş Olamaz.");
        RuleFor(p => p.UserId).NotNull().WithMessage("Kullanıcı Id Boş Olamaz");


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
        RuleFor(p => p.CurrentPassword).NotNull().WithMessage("Şifre Boş Olamaz");
        RuleFor(p => p.CurrentPassword).NotEmpty().WithMessage("Şifre Boş Olamaz");
        RuleFor(p => p.CurrentPassword).MinimumLength(8).WithMessage("Şifre En Az 8 Karakter Olmalıdır.");
        RuleFor(p => p.CurrentPassword).Matches("[A-Z]").WithMessage("Şifre En Az 1 Büyük Harf İçermelidir.");
        RuleFor(p => p.CurrentPassword).Matches("[a-z]").WithMessage("Şifre En Az 1 Küçük Harf İçermelidir.");
        RuleFor(p => p.CurrentPassword).Matches("[^a-zA-z0-9]").WithMessage("Şifre En Az 1 Adet Özel Karakter İçermelidir.");



        RuleFor(p => p.NewPassword).NotNull().WithMessage("Şifre Boş Olamaz");
        RuleFor(p => p.NewPassword).NotEmpty().WithMessage("Şifre Boş Olamaz");
        RuleFor(p => p.NewPassword).MinimumLength(8).WithMessage("Şifre En Az 8 Karakter Olmalıdır.");
        RuleFor(p => p.NewPassword).Matches("[A-Z]").WithMessage("Şifre En Az 1 Büyük Harf İçermelidir.");
        RuleFor(p => p.NewPassword).Matches("[a-z]").WithMessage("Şifre En Az 1 Küçük Harf İçermelidir.");
        RuleFor(p => p.NewPassword).Matches("[^a-zA-z0-9]").WithMessage("Şifre En Az 1 Adet Özel Karakter İçermelidir.");




        RuleFor(p => p.ConfirmNewPassword).NotNull().WithMessage("Şifre Boş Olamaz");
        RuleFor(p => p.ConfirmNewPassword).NotEmpty().WithMessage("Şifre Boş Olamaz");
        RuleFor(p => p.ConfirmNewPassword).MinimumLength(8).WithMessage("Şifre En Az 8 Karakter Olmalıdır.");
        RuleFor(p => p.ConfirmNewPassword).Matches("[A-Z]").WithMessage("Şifre En Az 1 Büyük Harf İçermelidir.");
        RuleFor(p => p.ConfirmNewPassword).Matches("[a-z]").WithMessage("Şifre En Az 1 Küçük Harf İçermelidir.");
        RuleFor(p => p.ConfirmNewPassword).Matches("[^a-zA-z0-9]").WithMessage("Şifre En Az 1 Adet Özel Karakter İçermelidir.");



        RuleFor(p => p.Phone).NotEmpty().WithMessage("Kullanıcı Telefon Numarası Boş Olamaz.");
        RuleFor(p => p.Phone).NotNull().WithMessage("Kullanıcı Telefon Numarası Boş Olamaz");
        RuleFor(p => p.Phone).MinimumLength(11).WithMessage("Kullanıcı Telefon Numarası En Az 11 Karakter Olmalıdır.");
        RuleFor(p => p.Phone).Matches(@"^[0-9]+$").WithMessage("Kullanıcı Telefon Numarası Sadece Rakamlardan Oluşmalıdır.");
        RuleFor(p => p.Phone).MaximumLength(15).WithMessage("Kullanıcı Telefon Numarası En Fazla 15 Karakterden Oluşmalıdır.");




    }
}
