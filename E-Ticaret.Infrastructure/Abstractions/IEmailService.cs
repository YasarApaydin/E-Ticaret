namespace E_Ticaret.Infrastructure.Abstractions
{
    public  interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
