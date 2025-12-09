namespace E_Ticaret.Domain.Common;
    public  record PaymentRequestDto
    {

    public Guid OrderId { get; init; }
    public string CardHolderName { get; init; } = null!;
    public string CardNumber { get; init; } = null!; 
    public string ExpireMonth { get; init; } = null!;
    public string ExpireYear { get; init; } = null!;
    public string Cvc { get; init; } = null!;
    public int Installments { get; init; } = 1;
    public string IpAddress { get; init; } = null!;

}

