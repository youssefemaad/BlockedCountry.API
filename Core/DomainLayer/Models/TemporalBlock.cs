namespace DomainLayer.Models
{
    public class TemporalBlock
    {
        public string CountryCode { get; set; } = string.Empty;
        public DateTime UnblockAt { get; set; }
    }
}