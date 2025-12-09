using E_Ticaret.Domain.Common;
using E_Ticaret.Infrastructure.Abstractions;

namespace E_Ticaret.Persistence.Services;
internal sealed class IyzicoPaymentProvider : IPaymentProvider
{


    public Task<External3DSCompleteResponse> Complete3DSAsync(string conversationId, string conversationData, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ExternalPaymentResponse> PayAsync(PaymentRequestDto request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<External3DSResponse> Start3DSAsync(PaymentRequestDto request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateWebhookAsync(string body, IDictionary<string, string> headers)
    {
        throw new NotImplementedException();
    }
}

