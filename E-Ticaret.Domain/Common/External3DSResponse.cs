namespace E_Ticaret.Domain.Common
{
    public class External3DSResponse
    {
        public bool IsRedirectRequired { get; set; }
        public string? RedirectUrl { get; set; }
        public string? ProviderPaymentId { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
