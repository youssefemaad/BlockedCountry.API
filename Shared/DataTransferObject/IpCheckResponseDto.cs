namespace Shared.DataTransferObject
{
    public class IpCheckResponseDto
    {
        public string IpAddress { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }
    }
}
