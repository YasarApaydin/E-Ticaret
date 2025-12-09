namespace E_Ticaret.Infrastructure.Options;
public sealed class KeycloakConfiguration
{
    public string Realm { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string HostName { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
}
