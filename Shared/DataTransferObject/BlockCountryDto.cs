namespace Shared.DataTransferObject
{
    public class BlockCountryDto
    {
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public int? BlockDurationMinutes { get; set; }
    }
}
