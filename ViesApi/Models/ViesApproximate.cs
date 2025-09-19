using System.Text.Json.Serialization;

namespace ViesApi.Models;

public class ViesApproximate
{
    [JsonPropertyName("name")]
    public string name { get; set; }

    [JsonPropertyName("street")]
    public string street { get; set; }

    [JsonPropertyName("postalCode")]
    public string postalCode { get; set; }

    [JsonPropertyName("city")]
    public string city { get; set; }

    [JsonPropertyName("companyType")]
    public string companyType { get; set; }

    [JsonPropertyName("matchName")]
    public int matchName { get; set; }

    [JsonPropertyName("matchStreet")]
    public int matchStreet { get; set; }

    [JsonPropertyName("matchPostalCode")]
    public int matchPostalCode { get; set; }

    [JsonPropertyName("matchCity")]
    public int matchCity { get; set; }

    [JsonPropertyName("matchCompanyType")]
    public int matchCompanyType { get; set; }
}