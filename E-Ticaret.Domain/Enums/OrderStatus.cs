namespace E_Ticaret.Domain.Enums
{
    public enum OrderStatus
    {
        Pending = 1,        // Bekliyor
        Processing = 2,     // Hazırlanıyor
        Shipped = 3,        // Kargolandı
        Delivered = 4,      // Teslim edildi
        Cancelled = 5       // İptal edildi

    }
}
