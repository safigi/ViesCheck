using System.Text.Json.Serialization;

namespace ViesApi.Models;

public class ErrorWrapper
{
    [JsonPropertyName("error")]
    public ViesError error { get; set; }
}