# ViesApi NuGet Package

A .NET library for validating VAT numbers using the European Commission's VIES service.

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

## Features

- Validate VAT numbers against the VIES API
- Format VAT numbers according to country-specific rules
- Get country-specific VAT number examples and formats
- Check the status of the VIES service

## License

This project is licensed under the MIT License.