# ViesCheck - VIES API NuGet Package

A .NET library for validating VAT numbers using the European
Commission's VIES service.\
This package is especially useful for companies where **tax audits** may
require official verification and the use of the **reference number**
(request identifier) returned by the VIES service. This identifier can
serve as evidence that the VAT number check was performed.

[![GitHub
Repository](https://img.shields.io/badge/GitHub-ViesCheck-blue?style=flat-square&logo=github)](https://github.com/safigi/ViesCheck)

------------------------------------------------------------------------

## Installation

Install the package via NuGet:

``` bash
dotnet add package ViesApi
```

------------------------------------------------------------------------

## Usage

### Configuration

Configure the VIES API service in your `Program.cs` or `Startup.cs`:

``` csharp
builder.Services.AddViesApiServices(config =>
{
    config.ApiEndpoint = "https://ec.europa.eu/taxation_customs/vies/rest-api";
    config.TimeoutSeconds = 30;
    config.UseProxy = false;
    config.ProxyUrl = string.Empty;
});
```

------------------------------------------------------------------------

### Service Injection

Inject the `IViesApiService` and `ViesVatFormatService` into your
classes:

``` csharp
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

------------------------------------------------------------------------

### Checking VAT Numbers

Use the `CheckVatNumberAsync` method to validate a VAT number:

``` csharp
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
    Console.WriteLine($"Reference Number: {response.RequestIdentifier}");
}
else
{
    Console.WriteLine($"VAT number is invalid: {response.ErrorMessage}");
}
```

⚠️ **Important:** Always store the `RequestIdentifier` (reference
number).\
This number proves that the VAT validation was executed through the VIES
system and may be required during **tax authority audits**.

------------------------------------------------------------------------

### Response Model

The `ViesCheckResponse` contains the following fields:

``` csharp
public class ViesCheckResponse
{
    public string CountryCode { get; set; }
    public string VatNumber { get; set; }
    public DateTime RequestDate { get; set; }
    public bool Valid { get; set; }
    public string RequestIdentifier { get; set; } // Reference number to keep for audits
    public string Name { get; set; }
    public string Address { get; set; }
    public string TraderName { get; set; }
    public string TraderStreet { get; set; }
    public string TraderPostalCode { get; set; }
    public string TraderCity { get; set; }
    public string TraderCompanyType { get; set; }

    public MatchType TraderNameMatch { get; set; }
    public MatchType TraderStreetMatch { get; set; }
    public MatchType TraderPostalCodeMatch { get; set; }
    public MatchType TraderCityMatch { get; set; }
    public MatchType TraderCompanyTypeMatch { get; set; }

    public bool HasError { get; set; }
    public string ErrorMessage { get; set; }
}
```

Key property:\
- **`RequestIdentifier`** → unique reference number from the VIES
system.\
Use this value for **audit documentation and compliance purposes**.

------------------------------------------------------------------------

### Audit Compliance

To comply with audit requirements, the `RequestIdentifier` should be
stored securely in your system logs or database.\
This ensures that in case of a **tax authority review**, you can present
the official VIES reference number proving that the VAT validation took
place.

Example of storing the reference number in a database:

``` csharp
public async Task SaveViesResultAsync(ViesCheckResponse response)
{
    var record = new ViesAuditLog
    {
        VatNumber = response.VatNumber,
        CountryCode = response.CountryCode,
        RequestDate = response.RequestDate,
        ReferenceNumber = response.RequestIdentifier,
        IsValid = response.Valid,
        TraderName = response.Name,
        Address = response.Address
    };

    _dbContext.ViesAuditLogs.Add(record);
    await _dbContext.SaveChangesAsync();
}
```

**Best practices:** - Always log the `RequestIdentifier` together with
the VAT number.\
- Store the `RequestDate` to prove when the validation was performed.\
- Keep records for at least the retention period required by your local
tax authority.

------------------------------------------------------------------------

### Formatting VAT Numbers

Use the `ViesVatFormatService` to format VAT numbers according to
country-specific rules:

``` csharp
var formattedVat = _vatFormatService.FormatVatNumber("12345678", "HU");
Console.WriteLine(formattedVat); // Output: HU12345678
```

------------------------------------------------------------------------

### Multi-language Country Names

The package supports country names in multiple languages:

``` csharp
// Get country name in English (default)
var englishName = _vatFormatService.GetCountryName("HU"); // "Hungary"

// Get country name in Hungarian
var hungarianName = _vatFormatService.GetCountryName("HU", "hu"); // "Magyarország"

// Get country name in German
var germanName = _vatFormatService.GetCountryName("DE", "de"); // "Deutschland"
```

------------------------------------------------------------------------

## Features

-   Validate VAT numbers against the VIES API\
-   **Retrieve and store the VIES Reference Number (RequestIdentifier)
    for tax audits**\
-   Format VAT numbers according to country-specific rules\
-   Get country-specific VAT number examples and formats\
-   Check the status of the VIES service\
-   Multi-language support for country names (24 languages)\
-   Clean Code and SOLID principles implementation\
-   Comprehensive unit test coverage

------------------------------------------------------------------------

## Testing

### Running Unit Tests

``` bash
dotnet test ViesApi.Tests
```

### Running Test Console Application

``` bash
cd ViesApi.TestConsole
dotnet run
```

The test console application demonstrates: - Multi-language country name
retrieval\
- VAT number formatting for different countries\
- VIES API status checking\
- Sample VAT number validation (with reference number logging)\
- All available features in action

------------------------------------------------------------------------

## Development

### Project Structure

    ViesApi/                   # Main NuGet package
    ├── Configuration/         # Configuration classes
    ├── Extensions/            # Dependency injection extensions
    ├── Interfaces/            # Service interfaces
    ├── Models/                # Data transfer objects
    └── Services/              # Service implementations

    Tests/                     # Unit tests (xUnit)
    TestConsole/               # Demo console application

------------------------------------------------------------------------

## License

This project is licensed under the MIT License.
