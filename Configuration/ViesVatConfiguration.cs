namespace ViesApi;

public static class ViesVatConfiguration
{
    public static readonly Dictionary<string, CountryVatFormat> Formats = new Dictionary<string, CountryVatFormat>
    {
        { "AT", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Austria"}, {"hu", "Ausztria"}, {"de", "Österreich"} }, 
            Format = "U########", Example = "ATU12345678" } },
        { "BE", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Belgium"}, {"hu", "Belgium"}, {"fr", "Belgique"}, {"nl", "België"} }, 
            Format = "##########", Example = "BE1234567890" } },
        { "BG", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Bulgaria"}, {"hu", "Bulgária"}, {"bg", "България"} }, 
            Format = "#########", Example = "BG123456789" } },
        { "HR", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Croatia"}, {"hu", "Horvátország"}, {"hr", "Hrvatska"} }, 
            Format = "###########", Example = "HR12345678901" } },
        { "CY", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Cyprus"}, {"hu", "Ciprus"}, {"el", "Κύπρος"} }, 
            Format = "########L", Example = "CY12345678X" } },
        { "CZ", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Czech Republic"}, {"hu", "Csehország"}, {"cs", "Česká republika"} }, 
            Format = "########", Example = "CZ12345678" } },
        { "DK", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Denmark"}, {"hu", "Dánia"}, {"da", "Danmark"} }, 
            Format = "########", Example = "DK12345678" } },
        { "EE", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Estonia"}, {"hu", "Észtország"}, {"et", "Eesti"} }, 
            Format = "#########", Example = "EE123456789" } },
        { "FI", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Finland"}, {"hu", "Finnország"}, {"fi", "Suomi"}, {"sv", "Finland"} }, 
            Format = "########", Example = "FI12345678" } },
        { "FR", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "France"}, {"hu", "Franciaország"}, {"fr", "France"} }, 
            Format = "X##########", Example = "FRX1234567890" } },
        { "DE", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Germany"}, {"hu", "Németország"}, {"de", "Deutschland"} }, 
            Format = "#########", Example = "DE123456789" } },
        { "EL", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Greece"}, {"hu", "Görögország"}, {"el", "Ελλάδα"} }, 
            Format = "#########", Example = "EL123456789" } },
        { "HU", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Hungary"}, {"hu", "Magyarország"} }, 
            Format = "########", Example = "HU12345678" } },
        { "IE", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Ireland"}, {"hu", "Írország"}, {"ga", "Éire"} }, 
            Format = "#######L", Example = "IE1234567WA" } },
        { "IT", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Italy"}, {"hu", "Olaszország"}, {"it", "Italia"} }, 
            Format = "###########", Example = "IT12345678901" } },
        { "LV", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Latvia"}, {"hu", "Lettország"}, {"lv", "Latvija"} }, 
            Format = "###########", Example = "LV12345678901" } },
        { "LT", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Lithuania"}, {"hu", "Litvánia"}, {"lt", "Lietuva"} }, 
            Format = "#########", Example = "LT123456789" } },
        { "LU", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Luxembourg"}, {"hu", "Luxemburg"}, {"fr", "Luxembourg"}, {"de", "Luxemburg"} }, 
            Format = "########", Example = "LU12345678" } },
        { "MT", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Malta"}, {"hu", "Málta"}, {"mt", "Malta"} }, 
            Format = "########", Example = "MT12345678" } },
        { "NL", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Netherlands"}, {"hu", "Hollandia"}, {"nl", "Nederland"} }, 
            Format = "#########B##", Example = "NL123456789B01" } },
        { "PL", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Poland"}, {"hu", "Lengyelország"}, {"pl", "Polska"} }, 
            Format = "##########", Example = "PL1234567890" } },
        { "PT", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Portugal"}, {"hu", "Portugália"}, {"pt", "Portugal"} }, 
            Format = "#########", Example = "PT123456789" } },
        { "RO", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Romania"}, {"hu", "Románia"}, {"ro", "România"} }, 
            Format = "##########", Example = "RO1234567890" } },
        { "SK", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Slovakia"}, {"hu", "Szlovákia"}, {"sk", "Slovensko"} }, 
            Format = "##########", Example = "SK1234567890" } },
        { "SI", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Slovenia"}, {"hu", "Szlovénia"}, {"sl", "Slovenija"} }, 
            Format = "########", Example = "SI12345678" } },
        { "ES", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Spain"}, {"hu", "Spanyolország"}, {"es", "España"} }, 
            Format = "X########", Example = "ESX12345678" } },
        { "SE", new CountryVatFormat { 
            CountryNames = new Dictionary<string, string> { {"en", "Sweden"}, {"hu", "Svédország"}, {"sv", "Sverige"} }, 
            Format = "############", Example = "SE123456789012" } }
    };
    
    /// <summary>
    /// Gets all available countries with their names in the specified language
    /// </summary>
    /// <param name="languageCode">Language code (e.g., "en", "hu", "de")</param>
    /// <returns>Dictionary with country codes and localized names</returns>
    public static Dictionary<string, string> GetCountryNames(string languageCode = "en")
    {
        return Formats.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.GetCountryName(languageCode)
        );
    }
    
    /// <summary>
    /// Gets supported language codes
    /// </summary>
    /// <returns>List of supported language codes</returns>
    public static List<string> GetSupportedLanguages()
    {
        var languages = new HashSet<string>();
        foreach (var format in Formats.Values)
        {
            foreach (var lang in format.CountryNames.Keys)
            {
                languages.Add(lang);
            }
        }
        return languages.OrderBy(l => l).ToList();
    }
}

public class CountryVatFormat
{
    public Dictionary<string, string> CountryNames { get; set; } = new Dictionary<string, string>();
    public string Format { get; set; }
    public string Example { get; set; }
    
    /// <summary>
    /// Gets the country name in the specified language. Falls back to English if language not found.
    /// </summary>
    /// <param name="languageCode">Language code (e.g., "en", "hu", "de")</param>
    /// <returns>Country name in specified language</returns>
    public string GetCountryName(string languageCode = "en")
    {
        if (CountryNames.TryGetValue(languageCode.ToLower(), out string name))
            return name;
        
        // Fallback to English if available
        if (CountryNames.TryGetValue("en", out string englishName))
            return englishName;
        
        // Fallback to first available name
        return CountryNames.Values.FirstOrDefault() ?? string.Empty;
    }
    
    /// <summary>
    /// Legacy property for backward compatibility. Returns English name.
    /// </summary>
    [Obsolete("Use GetCountryName() method instead for better language support")]
    public string CountryName => GetCountryName("en");
}
