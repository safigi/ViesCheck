using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ViesApi.Extensions;
using ViesApi.Interfaces;
using ViesApi.Models;
using ViesApi.Services;

namespace TestConsole;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== VIES API Test Console ===");
        Console.WriteLine();

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddViesApiServices(config =>
                {
                    config.BaseUrl = "https://ec.europa.eu/taxation_customs/vies/rest-api";
                    config.TimeoutSeconds = 30;
                    config.UserAgent = "ViesApi-TestConsole/1.0";
                });
            })
            .ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
            })
            .Build();

        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        var vatFormatService = host.Services.GetRequiredService<ViesVatFormatService>();
        var viesApiService = host.Services.GetRequiredService<IViesApiService>();

        logger.LogInformation("Starting VIES API test application");

        await TestVatFormatting(vatFormatService);
        await TestViesApiValidation(viesApiService, logger);

        logger.LogInformation("Test application completed successfully");
        Console.WriteLine("\n🎉 Test completed!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static Task TestVatFormatting(ViesVatFormatService vatFormatService)
    {
        Console.WriteLine("💯 VAT Number Formatting Test:");
        Console.WriteLine("─────────────────────────────────");

        (string, string?)[] testVatNumbers =
        [
            ("10773381", "HU"),
            ("U37893801", "AT"),
            ("123456789", "NL"),
            ("123456789", "DE"),
            ("X1234567890", "FR")
        ];

        foreach (var (number, country) in testVatNumbers)
        {
            var formatted = vatFormatService.FormatVatNumber(number, country);
            var example = vatFormatService.GetExampleVatNumber(country);
            Console.WriteLine($"{country}: {number} → {formatted} (Example: {example})");
        }

        var allCountries = vatFormatService.GetAllCountries("en");
        Console.WriteLine($"\nTotal countries: {allCountries.Count}");

        Console.WriteLine("\n🗣️ Supported Languages:");
        Console.WriteLine("─────────────────────");
        
        var languages = vatFormatService.GetSupportedLanguages();
        Console.WriteLine($"Total languages: {languages.Count}");
        Console.WriteLine($"Languages: {string.Join(", ", languages)}");

        return Task.CompletedTask;
    }

    static async Task TestViesApiValidation(IViesApiService viesApiService, ILogger logger)
    {
        Console.WriteLine("\n🌐 VIES API Status Check:");
        Console.WriteLine("────────────────────────");
        
        try
        {
            var statusResponse = await viesApiService.CheckStatusAsync();
            Console.WriteLine($"VIES Service Available: {statusResponse?.Vow?.Available ?? false}");
            
            if (statusResponse?.Countries != null)
            {
                Console.WriteLine($"Country services: {statusResponse.Countries.Count} countries");
                var availableCountries = statusResponse.Countries
                    .Where(c => c.Availability == CountryAvailability.Available)
                    .Count();
                Console.WriteLine($"Available countries: {availableCountries}");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to check VIES status");
            Console.WriteLine($"⚠️ Could not check VIES status: {ex.Message}");
        }

        Console.WriteLine("\n✅ Sample VAT Validation Test:");
        Console.WriteLine("─────────────────────────────");

        var sampleVatNumbers = new[]
        {
            new ViesCheckRequest { CountryCode = "HU", VatNumber = "10773381" },
            new ViesCheckRequest { CountryCode = "AT", VatNumber = "U37893801" }
        };

        foreach (var request in sampleVatNumbers)
        {
            try
            {
                Console.WriteLine($"\nTesting: {request.CountryCode}{request.VatNumber}");
                var response = await viesApiService.CheckVatNumberAsync(request);
                
                if (response.HasError)
                {
                    Console.WriteLine($"⚠️ Error: {response.ErrorMessage}");
                }
                else
                {
                    Console.WriteLine($"Valid: {response.Valid}");
                    if (response.Valid && !string.IsNullOrEmpty(response.Name))
                    {
                        Console.WriteLine($"Company: {response.Name}");
                        Console.WriteLine($"Address: {response.Address}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "VAT validation failed for {CountryCode}-{VatNumber}", 
                    request.CountryCode, request.VatNumber);
                Console.WriteLine($"⚠️ Validation failed: {ex.Message}");
            }
        }
    }
}
