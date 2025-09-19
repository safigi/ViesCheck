using System.Text.Json.Serialization;

namespace ViesApi.Models;

public class ViesErrorResponse
{
    [JsonPropertyName("errorWrapperError")]
    public ErrorWrapper errorWrapperError { get; set; }
}