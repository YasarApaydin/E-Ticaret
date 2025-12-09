namespace E_Ticaret.Domain.Common;
    public record CreatePaymentResultDto
    {
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public string PaymentId { get; init; } = null!;
    public string PaymentReference { get; init; } = null!;
    public Guid OrderId { get; init; }
    public DateTime PaidAt { get; init; }
    public bool Is3DSRequired { get; init; } = false;
    public string? ThreeDSHtmlContent { get; init; }
}
