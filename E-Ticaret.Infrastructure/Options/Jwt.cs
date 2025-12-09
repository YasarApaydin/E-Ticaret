namespace E_Ticaret.Infrastructure.Options
{
    public sealed class Jwt
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string SecretKey { get; set; } = default!;

    }
}
