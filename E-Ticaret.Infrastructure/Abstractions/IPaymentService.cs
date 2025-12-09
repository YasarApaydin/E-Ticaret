using E_Ticaret.Domain.Common;
using TS.Result;

namespace E_Ticaret.Infrastructure.Abstractions;
    public interface IPaymentService
    {
    Task<Result<CreatePaymentResultDto>> ProcessPaymentAsync(PaymentRequestDto request, CancellationToken cancellationToken);
    Task HandleIyzicoWebhookAsync(string body, IDictionary<string, string> headers, CancellationToken cancellationToken);
}

