namespace ViesApi.Models;

public class ViesCheckResponse
{
    public string CountryCode { get; set; }
    public string VatNumber { get; set; }
    public DateTime RequestDate { get; set; }
    public bool Valid { get; set; }
    public string RequestIdentifier { get; set; }
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
    public string? ErrorMessage { get; set; }
}