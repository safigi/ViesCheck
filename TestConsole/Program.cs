using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        // Setup Dependency Injection
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddViesApiServices(config =>
                {
                    config.ApiEndpoint = "https://ec.europa.eu/taxation_customs/vies/rest-api";
                    config.TimeoutSeconds = 30;
                });
                                services.AddSingleton<IViesVatFormatService, ViesVatFormatService>();
                services.AddHttpClient();
            })
            .Build();

        var vatFormatService = host.Services.GetRequiredService<IViesVatFormatService>();
        var viesApiService = host.Services.GetRequiredService<IViesApiService>();

        // Test 1: Multi-language country names
        Console.WriteLine("🌍 Multi-language Country Names Test:");
        Console.WriteLine("─────────────────────────────────────");
        
        var testCountries = new[] { "HU", "DE", "FR", "AT", "NL" };
        var testLanguages = new[] { "en", "hu", "de", "fr" };

        foreach (var country in testCountries)
        {
            Console.WriteLine($"\n{country}:");
            foreach (var lang in testLanguages)
            {
                var name = vatFormatService.GetCountryName(country, lang);
                Console.WriteLine($"  {lang}: {name}");
            }
        }

        // Test 2: VAT number formatting
        Console.WriteLine("\n\n💯 VAT Number Formatting Test:");
        Console.WriteLine("─────────────────────────────────");

        var testVatNumbers = new[]
        {
            ("10773381", "HU"),
            ("U37893801", "AT"),
            ("123456789", "NL"),
            ("123456789", "DE"),
            ("X1234567890", "FR")
        };

        foreach (var (number, country) in testVatNumbers)
        {
            var formatted = vatFormatService.FormatVatNumber(number, country);
            var example = vatFormatService.GetExampleVatNumber(country);
            Console.WriteLine($"{country}: {number} → {formatted} (Example: {example})");
        }

        // Test 3: Get all countries in different languages
        Console.WriteLine("\n\n📋 All Countries Test:");
        Console.WriteLine("────────────────────");

        var allCountriesEn = vatFormatService.GetAllCountries("en");
        var allCountriesHu = vatFormatService.GetAllCountries("hu");

        Console.WriteLine($"Total countries: {allCountriesEn.Count}");
        Console.WriteLine("\nFirst 10 countries (EN vs HU):");
        
        for (int i = 0; i < Math.Min(10, allCountriesEn.Count); i++)
        {
            var countryEn = allCountriesEn[i];
            var countryHu = allCountriesHu[i];
            Console.WriteLine($"{countryEn.Code}: {countryEn.Name} | {countryHu.Name}");
        }

        // Test 4: Supported languages
        Console.WriteLine("\n\n🗣️ Supported Languages:");
        Console.WriteLine("─────────────────────");
        
        var languages = vatFormatService.GetSupportedLanguages();
        Console.WriteLine($"Total languages: {languages.Count}");
        Console.WriteLine($"Languages: {string.Join(", ", languages)}");

        // Test 5: VIES API Status Check (if internet available)
        Console.WriteLine("\n\n🌐 VIES API Status Check:");
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
            Console.WriteLine($"⚠️ Could not check VIES status: {ex.Message}");
        }

        // Test 6: Sample VAT number validation (if internet available)
        Console.WriteLine("\n\n✅ Sample VAT Validation Test:");
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
                Console.WriteLine($"⚠️ Validation failed: {ex.Message}");
            }
        }

        Console.WriteLine("\n\n🎉 Test completed!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}