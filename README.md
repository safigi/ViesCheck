# ViesCheck - VIES API NuGet Package

A .NET library for validating VAT numbers using the European Commission's VIES service.

This package is especially useful for companies where **tax audits** may require official verification and the use of the **reference number** (request identifier) returned by the VIES service. This identifier can serve as evidence that the VAT number check was performed.

[![GitHub Repository](https://img.shields.io/badge/GitHub-ViesCheck-blue?style=flat-square&logo=github)](https://github.com/safigi/ViesCheck)

## Features

- ✅ Validate VAT numbers against the VIES API
- ✅ **Retrieve and store the VIES Reference Number (RequestIdentifier) for tax audits**
- ✅ Format VAT numbers according to country-specific rules
- ✅ Get country-specific VAT number examples and formats
- ✅ Check the status of the VIES service
- ✅ Multi-language support for country names (24 languages)
- ✅ **Configurable API settings** (BaseUrl, Timeout, UserAgent)
- ✅ **Built-in logging** for validation operations
- ✅ Clean Code, SOLID principles implementation
- ✅ Comprehensive unit test coverage

---

## Installation

Install the package via NuGet:

```bash
dotnet add package ViesApi
```

---

## Quick Start

### 1. Configuration

Configure the VIES API service in your `Program.cs`:

```csharp
using ViesApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add VIES API services with configuration
builder.Services.AddViesApiServices(config =>
{
    config.BaseUrl = "https://ec.europa.eu/taxation_customs/vies/rest-api";
    config.TimeoutSeconds = 30;
    config.UserAgent = "MyApp/1.0";
});

// Add logging
builder.Logging.AddConsole();

var app = builder.Build();
```

### 2. Service Injection

Inject the services into your classes:

```csharp
public class VatValidationService
{
    private readonly IViesApiService _viesApiService;
    private readonly ViesVatFormatService _vatFormatService;
    private readonly ILogger<VatValidationService> _logger;

    public VatValidationService(
        IViesApiService viesApiService, 
        ViesVatFormatService vatFormatService,
        ILogger<VatValidationService> logger)
    {
        _viesApiService = viesApiService;
        _vatFormatService = vatFormatService;
        _logger = logger;
    }
}
```

### 3. Validate VAT Numbers

```csharp
public async Task<VatValidationResult> ValidateVatAsync(string countryCode, string vatNumber)
{
    // Format the VAT number
    var formattedVat = _vatFormatService.FormatVatNumber(vatNumber, countryCode);
    
    // Create validation request
    var request = new ViesCheckRequest
    {
        CountryCode = countryCode,
        VatNumber = formattedVat
    };

    // Validate with VIES
    var response = await _viesApiService.CheckVatNumberAsync(request);

    if (response.Valid)
    {
        _logger.LogInformation("VAT {CountryCode}-{VatNumber} is valid for {CompanyName}", 
            countryCode, vatNumber, response.Name);
        
        // ⚠️ IMPORTANT: Store the RequestIdentifier for audit purposes
        await SaveAuditRecord(response);
        
        return new VatValidationResult
        {
            IsValid = true,
            CompanyName = response.Name,
            Address = response.Address,
            ReferenceNumber = response.RequestIdentifier // Store this!
        };
    }
    else
    {
        _logger.LogWarning("VAT {CountryCode}-{VatNumber} is invalid: {Error}", 
            countryCode, vatNumber, response.ErrorMessage);
        
        return new VatValidationResult
        {
            IsValid = false,
            ErrorMessage = response.ErrorMessage
        };
    }
}
```

---

## Configuration Options

The `ViesApiConfiguration` class provides these settings:

```csharp
public class ViesApiConfiguration
{
    public string BaseUrl { get; set; } = "https://ec.europa.eu/taxation_customs/vies/rest-api";
    public int TimeoutSeconds { get; set; } = 30;
    public string UserAgent { get; set; } = "ViesApi/1.0";
}
```

**Example configurations:**

```csharp
// Development environment
services.AddViesApiServices(config =>
{
    config.BaseUrl = "https://ec.europa.eu/taxation_customs/vies/rest-api";
    config.TimeoutSeconds = 60; // Longer timeout for development
    config.UserAgent = "MyApp-Dev/1.0";
});

// Production environment
services.AddViesApiServices(config =>
{
    config.TimeoutSeconds = 15; // Faster timeout for production
    config.UserAgent = "MyApp-Prod/2.1";
});
```

---

## Advanced Usage

### VAT Number Formatting

The service automatically formats VAT numbers according to country-specific rules:

```csharp
// Hungarian VAT number
var formatted = _vatFormatService.FormatVatNumber("12345678", "HU");
Console.WriteLine(formatted); // Output: HU12345678

// Austrian VAT number (adds 'U' prefix)
var formatted = _vatFormatService.FormatVatNumber("37893801", "AT");
Console.WriteLine(formatted); // Output: ATU37893801

// Dutch VAT number (adds 'B' separator)
var formatted = _vatFormatService.FormatVatNumber("123456789", "NL");
Console.WriteLine(formatted); // Output: NL123456789B01
```

### Multi-language Country Names

```csharp
// Get country names in different languages
var englishName = _vatFormatService.GetCountryName("HU"); // "Hungary"
var hungarianName = _vatFormatService.GetCountryName("HU", "hu"); // "Magyarország"
var germanName = _vatFormatService.GetCountryName("DE", "de"); // "Deutschland"

// Get all supported languages
var languages = _vatFormatService.GetSupportedLanguages();
// Returns: ["bg", "cs", "da", "de", "el", "en", "es", "et", "fi", "fr", "ga", "hr", "hu", "it", "lt", "lv", "mt", "nl", "pl", "pt", "ro", "sk", "sl", "sv"]

// Get all countries in a specific language
var allCountries = _vatFormatService.GetAllCountries("hu");
foreach (var country in allCountries)
{
    Console.WriteLine($"{country.Code}: {country.Name} (Example: {country.Example})");
}
```

### VIES Service Status

```csharp
public async Task<bool> IsViesAvailableAsync()
{
    var status = await _viesApiService.CheckStatusAsync();
    return status.Vow?.Available ?? false;
}
```

### Validation with Requester Information

For enhanced validation, you can include your own VAT information:

```csharp
var request = new ViesCheckRequest
{
    CountryCode = "HU",
    VatNumber = "12345678",
    RequesterMemberStateCode = "DE", // Your country code
    RequesterNumber = "123456789"     // Your VAT number
};

var response = await _viesApiService.CheckVatNumberAsync(request);

// This provides additional trader matching information
if (response.Valid)
{
    Console.WriteLine($"Name match: {response.TraderNameMatch}");
    Console.WriteLine($"Address match: {response.TraderStreetMatch}");
}
```

---

## Audit Compliance & Logging

### Built-in Logging

The library provides automatic logging for all validation operations:

```
[14:23:16 INF] Starting VAT validation for HU-12345678
[14:23:17 INF] VAT validation completed for HU-12345678: True
```

### Storing Audit Records

**⚠️ CRITICAL:** Always store the `RequestIdentifier` for audit compliance:

```csharp
public async Task SaveAuditRecord(ViesCheckResponse response)
{
    var auditRecord = new VatAuditLog
    {
        VatNumber = response.VatNumber,
        CountryCode = response.CountryCode,
        RequestDate = response.RequestDate,
        ReferenceNumber = response.RequestIdentifier, // This is required for audits!
        IsValid = response.Valid,
        CompanyName = response.Name,
        CompanyAddress = response.Address,
        ValidationTimestamp = DateTime.UtcNow
    };

    await _auditRepository.SaveAsync(auditRecord);
    
    _logger.LogInformation("Audit record saved for VAT {CountryCode}-{VatNumber} with reference {ReferenceNumber}",
        response.CountryCode, response.VatNumber, response.RequestIdentifier);
}
```

### Audit Best Practices

- ✅ **Always log the `RequestIdentifier`** together with the VAT number
- ✅ **Store the `RequestDate`** to prove when validation was performed  
- ✅ **Keep records** for at least the retention period required by your local tax authority
- ✅ **Use structured logging** to make audit trails searchable
- ✅ **Store validation results** even for invalid VAT numbers

---

## Response Model

The `ViesCheckResponse` contains comprehensive validation information:

```csharp
public class ViesCheckResponse
{
    // Basic validation info
    public string CountryCode { get; set; }
    public string VatNumber { get; set; }
    public DateTime RequestDate { get; set; }
    public bool Valid { get; set; }
    
    // 🔥 AUDIT CRITICAL: Store this reference number!
    public string RequestIdentifier { get; set; } 
    
    // Company information
    public string Name { get; set; }
    public string Address { get; set; }
    
    // Trader verification (when using requester info)
    public string TraderName { get; set; }
    public string TraderStreet { get; set; }
    public string TraderPostalCode { get; set; }
    public string TraderCity { get; set; }
    public string TraderCompanyType { get; set; }

    // Match scores for trader verification
    public MatchType TraderNameMatch { get; set; }
    public MatchType TraderStreetMatch { get; set; }
    public MatchType TraderPostalCodeMatch { get; set; }
    public MatchType TraderCityMatch { get; set; }
    public MatchType TraderCompanyTypeMatch { get; set; }

    // Error handling
    public bool HasError { get; set; }
    public string ErrorMessage { get; set; }
}
```

---

## Testing

### Running Unit Tests

```bash
dotnet test
```

### Running Test Console Application

```bash
cd TestConsole
dotnet run
```

The test console demonstrates:
- Multi-language country name retrieval
- VAT number formatting for different countries  
- VIES API status checking
- Sample VAT number validation with logging
- All available features in action

---

## Project Structure

```
ViesApi/                   # Main NuGet package
├── Configuration/         # Configuration classes  
├── Extensions/            # Dependency injection extensions
├── Interfaces/            # Service interfaces
├── Models/                # Request/Response models
└── Services/              # Service implementations

Tests/                     # Unit tests (xUnit)
TestConsole/               # Demo console application
scripts/                   # Version management scripts
.github/workflows/         # CI/CD automation
```

---

## Version Management

This project uses automated version synchronization:

- **Centralized versioning** via `Directory.Build.props`
- **Automatic NuGet packaging** on build
- **CI/CD integration** with GitHub Actions
- **Git tag-based releases**

To create a new release:
```bash
git tag v1.0.3
git push origin v1.0.3
# GitHub Actions automatically builds and publishes
```

---

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add/update tests
5. Submit a pull request

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## Support

- 📖 [Documentation](https://github.com/safigi/ViesCheck)
- 🐛 [Issues](https://github.com/safigi/ViesCheck/issues)
- 💬 [Discussions](https://github.com/safigi/ViesCheck/discussions)

---

**⚠️ Important Notice:** This library is designed for compliance with tax audit requirements. Always store the `RequestIdentifier` returned by VIES validation for audit trail purposes.
