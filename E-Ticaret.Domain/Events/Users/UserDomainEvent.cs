using E_Ticaret.Domain.Entities;
using MediatR;

namespace E_Ticaret.Domain.Events.Users
{
    public sealed class UserDomainEvent:INotification
    {



        public AppUser AppUser { get; }
        public string VerificationCode { get; }

        public UserDomainEvent(AppUser appUser, string verificationCode)
        {
            AppUser = appUser;
            VerificationCode = verificationCode;
        }


    }
}
