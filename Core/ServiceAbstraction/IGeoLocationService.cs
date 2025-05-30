namespace DomainLayer.Contracts;

public interface IGeoLocationService
{
    string GetCountryCodeByIp(string ipAddress);
}
