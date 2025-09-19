using System.Text.Json.Serialization;

namespace ViesApi.Models;

public class ViesError
{
    [JsonPropertyName("errorCode")]
    public string errorCode { get; set; }

    [JsonPropertyName("errorMessage")]
    public string errorMessage { get; set; }
}