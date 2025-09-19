namespace ViesApi.Models;

public class ViesCheckRequest
{
    public string CountryCode { get; set; }
    public string VatNumber { get; set; }
    public string RequesterMemberStateCode { get; set; }
    public string RequesterNumber { get; set; }
}