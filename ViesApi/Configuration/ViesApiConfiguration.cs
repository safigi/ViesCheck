namespace ViesApi.Configuration;

public class ViesApiConfiguration
{
    public string BaseUrl { get; set; } = "https://ec.europa.eu/taxation_customs/vies/rest-api";
    public int TimeoutSeconds { get; set; } = 30;
    public string UserAgent { get; set; } = "ViesApi/1.0";
}
