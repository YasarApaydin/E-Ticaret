using E_Ticaret.Domain.Common;
using E_Ticaret.Domain.Interfaces.Repositories;
using E_Ticaret.Infrastructure.Abstractions;
using TS.Result;

namespace E_Ticaret.Persistence.Services;
internal sealed class PaymentService : IPaymentService
{
    private readonly IPaymentRepository paymentRepository; 


    public Task HandleIyzicoWebhookAsync(string body, IDictionary<string, string> headers, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<CreatePaymentResultDto>> ProcessPaymentAsync(PaymentRequestDto request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
