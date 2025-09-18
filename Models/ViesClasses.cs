using System.Text.Json.Serialization;

namespace ViesApi;

public class CountryInfo
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Example { get; set; }
    public string Format { get; set; }
}

public enum MatchType
{
    VALID,
    INVALID,
    NOT_PROCESSED
}

public enum CountryAvailability
{
    Available,
    Unavailable,
    MonitoringDisabled
}

public class ViesCheckRequest
{
    public string CountryCode { get; set; }
    public string VatNumber { get; set; }
    public string RequesterMemberStateCode { get; set; }
    public string RequesterNumber { get; set; }
}

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
    public string ErrorMessage { get; set; }
}

public class ViesResponse
{
    [JsonPropertyName("isValid")]
    public bool isValid { get; set; }

    [JsonPropertyName("requestDate")]
    public string requestDate { get; set; }

    [JsonPropertyName("userError")]
    public string userError { get; set; }

    [JsonPropertyName("name")]
    public string name { get; set; }

    [JsonPropertyName("address")]
    public string address { get; set; }

    [JsonPropertyName("requestIdentifier")]
    public string requestIdentifier { get; set; }

    [JsonPropertyName("originalVatNumber")]
    public string originalVatNumber { get; set; }

    [JsonPropertyName("vatNumber")]
    public string vatNumber { get; set; }

    [JsonPropertyName("viesApproximate")]
    public ViesApproximate viesApproximate { get; set; }
}
public class ViesApproximate
{
    [JsonPropertyName("name")]
    public string name { get; set; }

    [JsonPropertyName("street")]
    public string street { get; set; }

    [JsonPropertyName("postalCode")]
    public string postalCode { get; set; }

    [JsonPropertyName("city")]
    public string city { get; set; }

    [JsonPropertyName("companyType")]
    public string companyType { get; set; }

    [JsonPropertyName("matchName")]
    public int matchName { get; set; }

    [JsonPropertyName("matchStreet")]
    public int matchStreet { get; set; }

    [JsonPropertyName("matchPostalCode")]
    public int matchPostalCode { get; set; }

    [JsonPropertyName("matchCity")]
    public int matchCity { get; set; }

    [JsonPropertyName("matchCompanyType")]
    public int matchCompanyType { get; set; }
}

public class ViesErrorResponse
{
    [JsonPropertyName("errorWrapperError")]
    public ErrorWrapper errorWrapperError { get; set; }
}

public class ErrorWrapper
{
    [JsonPropertyName("error")]
    public ViesError error { get; set; }
}

public class ViesError
{
    [JsonPropertyName("errorCode")]
    public string errorCode { get; set; }

    [JsonPropertyName("errorMessage")]
    public string errorMessage { get; set; }
}

public class CountryStatus
{
    public string CountryCode { get; set; }
    public CountryAvailability Availability { get; set; }
}

public class StatusInformationResponse
{
    public VowStatus Vow { get; set; }
    public List<CountryStatus> Countries { get; set; }
}

public class VowStatus
{
    public bool Available { get; set; }
}

public class CommonResponse
{
    public bool ActionSucceed { get; set; }
    public List<ErrorWrapper> ErrorWrappers { get; set; }
}
