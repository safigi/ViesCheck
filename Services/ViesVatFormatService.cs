namespace ViesApi;

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

    public List<CountryInfo> GetAllCountries()
    {
        var countries = new List<CountryInfo>();

        foreach (var country in ViesVatConfiguration.Formats)
        {
            countries.Add(new CountryInfo
            {
                Code = country.Key,
                Name = country.Value.CountryName,
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

    public string GetCountryName(string countryCode)
    {
        var config = GetVatConfig(countryCode);
        return config?.CountryName ?? countryCode;
    }

    public string GetExampleVatNumber(string countryCode)
    {
        var config = GetVatConfig(countryCode);
        return config?.Example ?? $"{countryCode}12345678";
    }
}
