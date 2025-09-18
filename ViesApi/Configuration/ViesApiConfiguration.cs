namespace ViesApi.Configuration;

public class ViesApiConfiguration
{
    public string ApiEndpoint { get; set; } = "https://ec.europa.eu/taxation_customs/vies/services/checkVatService";
    public int TimeoutSeconds { get; set; } = 30;
    public bool UseProxy { get; set; } = false;
    public string ProxyUrl { get; set; } = string.Empty;
}