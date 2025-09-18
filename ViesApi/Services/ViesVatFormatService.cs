using ViesApi.Configuration;
using ViesApi.Models;

namespace ViesApi.Services;

public class ViesVatFormatService
{
    public string FormatVatNumber(string vatNumber, string countryCode)
    {
        if (string.IsNullOrWhiteSpace(vatNumber) || string.IsNullOrWhiteSpace(countryCode))
            return vatNumber;

        vatNumber = vatNumber.Replace(" ", "").Replace("-", "").ToUpper();
        countryCode = countryCode.ToUpper();

        if (vatNumber.StartsWith(countryCode))
            return vatNumber;

        var vatConfig = GetVatConfig(countryCode);
        if (vatConfig != null)
        {
            return ApplyCountrySpecificFormatting(vatNumber, countryCode, vatConfig);
        }

        return countryCode + vatNumber;
    }

    private string ApplyCountrySpecificFormatting(string vatNumber, string countryCode, CountryVatFormat vatConfig)
    {
        switch (countryCode)
        {
            case "AT":
                if (!vatNumber.StartsWith("U"))
                    vatNumber = "U" + vatNumber;
                break;
            case "NL":
                if (vatNumber.Length >= 9 && !vatNumber.Contains("B"))
                {
                    var firstPart = vatNumber.Substring(0, 9);
                    var lastPart = vatNumber.Length > 9 ? vatNumber.Substring(9) : "01";
                    vatNumber = firstPart + "B" + lastPart.PadLeft(2, '0');
                }
                break;
        }

        return countryCode + vatNumber;
    }

    public List<CountryInfo> GetAllCountries(string languageCode = "en")
    {
        var countries = new List<CountryInfo>();

        foreach (var country in ViesVatConfiguration.Formats)
        {
            countries.Add(new CountryInfo
            {
                Code = country.Key,
                Name = country.Value.GetCountryName(languageCode),
                Example = country.Value.Example,
                Format = country.Value.Format
            });
        }

        return countries;
    }

    public CountryVatFormat GetVatConfig(string countryCode)
    {
        if (ViesVatConfiguration.Formats.TryGetValue(countryCode.ToUpper(), out var config))
        {
            return config;
        }
        return null;
    }

    public string GetCountryName(string countryCode, string languageCode = "en")
    {
        var config = GetVatConfig(countryCode);
        return config?.GetCountryName(languageCode) ?? countryCode;
    }

    public string GetExampleVatNumber(string countryCode)
    {
        var config = GetVatConfig(countryCode);
        return config?.Example ?? $"{countryCode}12345678";
    }

    /// <summary>
    /// Gets all supported language codes for country names
    /// </summary>
    /// <returns>List of language codes</returns>
    public List<string> GetSupportedLanguages()
    {
        return ViesVatConfiguration.GetSupportedLanguages();
    }

    /// <summary>
    /// Gets all country names in the specified language
    /// </summary>
    /// <param name="languageCode">Language code (e.g., "en", "hu", "de")</param>
    /// <returns>Dictionary with country codes and localized names</returns>
    public Dictionary<string, string> GetAllCountryNames(string languageCode = "en")
    {
        return ViesVatConfiguration.GetCountryNames(languageCode);
    }
}
