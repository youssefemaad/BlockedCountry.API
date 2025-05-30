namespace Shared.DataTransferObject
{
    public class BlockedAttemptDto
    {
        public string IpAddress { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public bool BlockedStatus { get; set; }
        public string UserAgent { get; set; } = string.Empty;
    }
}
