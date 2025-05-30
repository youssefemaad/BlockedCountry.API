namespace Core.Models
{
    /// <summary>
    /// Represents a blocked country with optional expiration
    /// </summary>
    public class BlockedCountry
    {
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }

        public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value <= DateTime.UtcNow;
    }
}
