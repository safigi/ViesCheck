# ViesCheck - VIES API NuGet Package

A .NET library for validating VAT numbers using the European Commission's VIES service.

[![GitHub Repository](https://img.shields.io/badge/GitHub-ViesCheck-blue?style=flat-square&logo=github)](https://github.com/safigi/ViesCheck)

## Installation

Install the package via NuGet:

```bash
dotnet add package ViesApi
```

## Usage

### Configuration

Configure the VIES API service in your `Program.cs` or `Startup.cs`:

```csharp
builder.Services.AddViesApiServices(config =>
{
    config.ApiEndpoint = "https://ec.europa.eu/taxation_customs/vies/rest-api";
    config.TimeoutSeconds = 30;
    config.UseProxy = false;
    config.ProxyUrl = string.Empty;
});
```

### Service Injection

Inject the `IViesApiService` and `ViesVatFormatService` into your classes:

```csharp
public class YourService
{
    private readonly IViesApiService _viesApiService;
    private readonly ViesVatFormatService _vatFormatService;

    public YourService(IViesApiService viesApiService, ViesVatFormatService vatFormatService)
    {
        _viesApiService = viesApiService;
        _vatFormatService = vatFormatService;
    }
}
```

### Checking VAT Numbers

Use the `CheckVatNumberAsync` method to validate a VAT number:

```csharp
var request = new ViesCheckRequest
{
    CountryCode = "HU",
    VatNumber = "12345678",
    RequesterMemberStateCode = "DE",
    RequesterNumber = "123456789"
};

var response = await _viesApiService.CheckVatNumberAsync(request);

if (response.Valid)
{
    Console.WriteLine($"VAT number is valid for {response.Name} at {response.Address}");
}
else
{
    Console.WriteLine($"VAT number is invalid: {response.ErrorMessage}");
}
```

### Formatting VAT Numbers

Use the `ViesVatFormatService` to format VAT numbers according to country-specific rules:

```csharp
var formattedVat = _vatFormatService.FormatVatNumber("12345678", "HU");
Console.WriteLine(formattedVat); // Output: HU12345678
```

### Multi-language Country Names

The package supports country names in multiple languages:

```csharp
// Get country name in English (default)
var englishName = _vatFormatService.GetCountryName("HU"); // "Hungary"

// Get country name in Hungarian
var hungarianName = _vatFormatService.GetCountryName("HU", "hu"); // "Magyarország"

// Get country name in German
var germanName = _vatFormatService.GetCountryName("DE", "de"); // "Deutschland"

// Get all countries with localized names
var countryNames = _vatFormatService.GetAllCountryNames("en");
foreach (var country in countryNames)
{
    Console.WriteLine($"{country.Key}: {country.Value}");
}

// Get all supported languages
var languages = _vatFormatService.GetSupportedLanguages();
// Returns: ["bg", "cs", "da", "de", "el", "en", "es", "et", "fi", "fr", "ga", "hr", "hu", "it", "lv", "lt", "mt", "nl", "pl", "pt", "ro", "sk", "sl", "sv"]
```

## Features

- Validate VAT numbers against the VIES API
- Format VAT numbers according to country-specific rules
- Get country-specific VAT number examples and formats
- Check the status of the VIES service
- Multi-language support for country names (24 languages)
- Clean Code and SOLID principles implementation
- Comprehensive unit test coverage

## Testing

### Running Unit Tests

```bash
dotnet test ViesApi.Tests
```

### Running Test Console Application

```bash
cd ViesApi.TestConsole
dotnet run
```

The test console application demonstrates:
- Multi-language country name retrieval
- VAT number formatting for different countries
- VIES API status checking
- Sample VAT number validation
- All available features in action

### Test Coverage

The project includes comprehensive unit tests covering:
- `ViesVatFormatService` - All formatting and utility methods
- `ViesVatConfiguration` - Multi-language configuration
- `CountryVatFormat` - Language fallback logic
- Edge cases and error handling

## Development

### Project Structure

```
ViesApi/                   # Main NuGet package
├── Configuration/         # Configuration classes
├── Extensions/           # Dependency injection extensions
├── Interfaces/           # Service interfaces
├── Models/              # Data transfer objects
└── Services/            # Service implementations

Tests/                    # Unit tests (xUnit)
TestConsole/             # Demo console application
```

## License

This project is licensed under the MIT License.