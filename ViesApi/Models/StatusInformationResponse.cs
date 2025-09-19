namespace ViesApi.Models;

public class StatusInformationResponse
{
    public VowStatus Vow { get; set; }
    public List<CountryStatus> Countries { get; set; }
}