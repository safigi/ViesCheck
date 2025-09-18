namespace ViesApi;

public static class ViesVatConfiguration
{
    public static readonly Dictionary<string, CountryVatFormat> Formats = new Dictionary<string, CountryVatFormat>
    {
        { "AT", new CountryVatFormat { CountryName = "Ausztria", Format = "U########", Example = "ATU12345678" } },
        { "BE", new CountryVatFormat { CountryName = "Belgium", Format = "##########", Example = "BE1234567890" } },
        { "BG", new CountryVatFormat { CountryName = "Bulgária", Format = "#########", Example = "BG123456789" } },
        { "HR", new CountryVatFormat { CountryName = "Horvátország", Format = "###########", Example = "HR12345678901" } },
        { "CY", new CountryVatFormat { CountryName = "Ciprus", Format = "########L", Example = "CY12345678X" } },
        { "CZ", new CountryVatFormat { CountryName = "Csehország", Format = "########", Example = "CZ12345678" } },
        { "DK", new CountryVatFormat { CountryName = "Dánia", Format = "########", Example = "DK12345678" } },
        { "EE", new CountryVatFormat { CountryName = "Észtország", Format = "#########", Example = "EE123456789" } },
        { "FI", new CountryVatFormat { CountryName = "Finnország", Format = "########", Example = "FI12345678" } },
        { "FR", new CountryVatFormat { CountryName = "Franciaország", Format = "X##########", Example = "FRX1234567890" } },
        { "DE", new CountryVatFormat { CountryName = "Németország", Format = "#########", Example = "DE123456789" } },
        { "EL", new CountryVatFormat { CountryName = "Görögország", Format = "#########", Example = "EL123456789" } },
        { "HU", new CountryVatFormat { CountryName = "Magyarország", Format = "########", Example = "HU12345678" } },
        { "IE", new CountryVatFormat { CountryName = "Írország", Format = "#######L", Example = "IE1234567WA" } },
        { "IT", new CountryVatFormat { CountryName = "Olaszország", Format = "###########", Example = "IT12345678901" } },
        { "LV", new CountryVatFormat { CountryName = "Lettország", Format = "###########", Example = "LV12345678901" } },
        { "LT", new CountryVatFormat { CountryName = "Litvánia", Format = "#########", Example = "LT123456789" } },
        { "LU", new CountryVatFormat { CountryName = "Luxemburg", Format = "########", Example = "LU12345678" } },
        { "MT", new CountryVatFormat { CountryName = "Málta", Format = "########", Example = "MT12345678" } },
        { "NL", new CountryVatFormat { CountryName = "Hollandia", Format = "#########B##", Example = "NL123456789B01" } },
        { "PL", new CountryVatFormat { CountryName = "Lengyelország", Format = "##########", Example = "PL1234567890" } },
        { "PT", new CountryVatFormat { CountryName = "Portugália", Format = "#########", Example = "PT123456789" } },
        { "RO", new CountryVatFormat { CountryName = "Románia", Format = "##########", Example = "RO1234567890" } },
        { "SK", new CountryVatFormat { CountryName = "Szlovákia", Format = "##########", Example = "SK1234567890" } },
        { "SI", new CountryVatFormat { CountryName = "Szlovénia", Format = "########", Example = "SI12345678" } },
        { "ES", new CountryVatFormat { CountryName = "Spanyolország", Format = "X########", Example = "ESX12345678" } },
        { "SE", new CountryVatFormat { CountryName = "Svédország", Format = "############", Example = "SE123456789012" } }
    };
}

public class CountryVatFormat
{
    public string CountryName { get; set; }
    public string Format { get; set; }
    public string Example { get; set; }
}
