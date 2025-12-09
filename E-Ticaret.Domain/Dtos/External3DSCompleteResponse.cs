namespace E_Ticaret.Domain.Common;
    public class External3DSCompleteResponse
    {
    public bool IsSuccess { get; set; }
    public string? ProviderPaymentId { get; set; }
    public string? ErrorMessage { get; set; }
}

