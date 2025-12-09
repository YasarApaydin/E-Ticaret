using E_Ticaret.Domain.Common;

namespace E_Ticaret.Infrastructure.Abstractions;
    public interface IPaymentProvider
    {
    Task<ExternalPaymentResponse> PayAsync(PaymentRequestDto request, CancellationToken cancellationToken);
    Task<External3DSResponse> Start3DSAsync(PaymentRequestDto request, CancellationToken cancellationToken);
    Task<External3DSCompleteResponse> Complete3DSAsync(string conversationId, string conversationData, CancellationToken cancellationToken);
    Task<bool> ValidateWebhookAsync(string body, IDictionary<string, string> headers);
}

