using ViesApi.Configuration;
using ViesApi.Models;

namespace ViesApi.Services;

public interface IViesVatFormatService
{
    string FormatVatNumber(string vatNumber, string countryCode);
    List<CountryInfo> GetAllCountries(string languageCode = "en");
    CountryVatFormat GetVatConfig(string countryCode);
    string GetCountryName(string countryCode, string languageCode = "en");
    string GetExampleVatNumber(string countryCode);
    List<string> GetSupportedLanguages();
    Dictionary<string, string> GetAllCountryNames(string languageCode = "en");
}
