using System.Text.Json.Serialization;

namespace ViesApi.Models;

public class ViesResponse
{
    [JsonPropertyName("isValid")]
    public bool isValid { get; set; }

    [JsonPropertyName("requestDate")]
    public string requestDate { get; set; }

    [JsonPropertyName("userError")]
    public string userError { get; set; }

    [JsonPropertyName("name")]
    public string name { get; set; }

    [JsonPropertyName("address")]
    public string address { get; set; }

    [JsonPropertyName("requestIdentifier")]
    public string requestIdentifier { get; set; }

    [JsonPropertyName("originalVatNumber")]
    public string originalVatNumber { get; set; }

    [JsonPropertyName("vatNumber")]
    public string vatNumber { get; set; }

    [JsonPropertyName("viesApproximate")]
    public ViesApproximate? viesApproximate { get; set; }
}