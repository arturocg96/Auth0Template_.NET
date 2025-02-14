namespace AuthTemplate.Configuration
{
    public class AuthSettings
    {
        public string Domain { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Issuer => $"https://{Domain}/";
    }
}
