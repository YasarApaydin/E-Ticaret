namespace E_Ticaret.Domain.Common;
    public class ExternalPaymentResponse
    {
    public bool IsSuccess { get; set; }
    public string? ProviderPaymentId { get; set; }
    public string? ErrorMessage { get; set; }
}

