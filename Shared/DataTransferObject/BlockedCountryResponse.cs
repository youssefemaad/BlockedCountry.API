namespace Shared.DataTransferObject
{
    public class BlockedCountryResponse
    {
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public DateTime? UnblockAt { get; set; }
    }
} 