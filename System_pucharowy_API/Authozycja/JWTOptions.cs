namespace System_pucharowy_API.Authozycja
{
    public class JWTOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string Key { get; set; } = default!;
        public int ExpiresMinutes { get; set; } = 240;
    }
}
