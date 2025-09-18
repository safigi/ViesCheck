using System.Text.Json;
using MatchType = ViesApi.MatchType;

namespace ViesApi;

public class ViesApiService : IViesApiService : IViesApiService : IViesApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://ec.europa.eu/taxation_customs/vies/rest-api";

    public ViesApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "EBP_V1_Application/1.0");
    }

    public async Task<ViesCheckResponse> CheckVatNumberAsync(ViesCheckRequest request)
    {
        try
        {
            if (!string.IsNullOrEmpty(request.RequesterMemberStateCode) && !string.IsNullOrEmpty(request.RequesterNumber))
            {
                return await CheckVatWithRequesterAsync(
                    request.CountryCode, 
                    request.VatNumber,
                    request.RequesterMemberStateCode,
                    request.RequesterNumber,
                    request);
            }
            else
            {
                return await CheckVatSimpleAsync(
                    request.CountryCode, 
                    request.VatNumber,
                    request);
            }
        }
        catch (Exception ex)
        {
            return new ViesCheckResponse
            {
                CountryCode = request.CountryCode,
                VatNumber = request.VatNumber,
                RequestDate = DateTime.Now,
                Valid = false,
                HasError = true,
                ErrorMessage = ex.Message
            };
        }
    }

    private async Task<ViesCheckResponse> CheckVatWithRequesterAsync(
        string targetCountryCode, 
        string targetVatNumber,
        string requesterCountryCode, 
        string requesterVatNumber, 
        ViesCheckRequest originalRequest)
    {
        try
        {
            string url = $"{_baseUrl}/ms/{targetCountryCode}/vat/{targetVatNumber}" +
                        $"?requesterMemberStateCode={requesterCountryCode}&requesterNumber={requesterVatNumber}";

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
                
            if (response.IsSuccessStatusCode)
            {
                var viesData = JsonSerializer.Deserialize<ViesResponse>(content);
                return MapViesResponseToVatCheckResponse(viesData, originalRequest);
            }
            else
            {
                var errorData = TryParseError(content);
                return new ViesCheckResponse
                {
                    CountryCode = targetCountryCode,
                    VatNumber = originalRequest.VatNumber,
                    RequestDate = DateTime.Now,
                    Valid = false,
                    HasError = true,
                    ErrorMessage = errorData?.ErrorMessage ?? $"VIES API Error: {response.StatusCode}"
                };
            }
        }
        catch (Exception ex)
        {
            return new ViesCheckResponse
            {
                CountryCode = targetCountryCode,
                VatNumber = originalRequest.VatNumber,
                RequestDate = DateTime.Now,
                Valid = false,
                HasError = true,
                ErrorMessage = ex.Message
            };
        }
    }

    private ViesCheckResponse MapViesResponseToVatCheckResponse(ViesResponse viesData, ViesCheckRequest originalRequest)
    {
        if (viesData == null)
        {
            return new ViesCheckResponse
            {
                CountryCode = originalRequest.CountryCode,
                VatNumber = originalRequest.VatNumber,
                RequestDate = DateTime.Now,
                Valid = false,
                HasError = true,
                ErrorMessage = "Could not parse VIES response"
            };
        }

        var response = new ViesCheckResponse
        {
            CountryCode = originalRequest.CountryCode,
            VatNumber = originalRequest.VatNumber,
            RequestDate = ParseRequestDate(viesData.requestDate),
            Valid = viesData.isValid,
            RequestIdentifier = viesData.requestIdentifier,
            Name = CleanViesValue(viesData.name),
            Address = CleanViesValue(viesData.address),
            HasError = false,
            ErrorMessage = null,                             
                
            TraderNameMatch = MatchType.NOT_PROCESSED,
            TraderStreetMatch = MatchType.NOT_PROCESSED,
            TraderPostalCodeMatch = MatchType.NOT_PROCESSED,
            TraderCityMatch = MatchType.NOT_PROCESSED,
            TraderCompanyTypeMatch = MatchType.NOT_PROCESSED
        };

        if (viesData.viesApproximate != null)
        {
            response.TraderNameMatch = ConvertMatchScore(viesData.viesApproximate.matchName);
            response.TraderStreetMatch = ConvertMatchScore(viesData.viesApproximate.matchStreet);
            response.TraderPostalCodeMatch = ConvertMatchScore(viesData.viesApproximate.matchPostalCode);
            response.TraderCityMatch = ConvertMatchScore(viesData.viesApproximate.matchCity);
            response.TraderCompanyTypeMatch = ConvertMatchScore(viesData.viesApproximate.matchCompanyType);
        }

        return response;
    }

    private DateTime ParseRequestDate(string requestDate)
    {
        if (DateTime.TryParse(requestDate, out DateTime result))
            return result;
        return DateTime.Now;
    }

    private string CleanViesValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value == "---")
            return null;
        return value.Trim();
    }

    private MatchType ConvertMatchScore(int matchScore)
    {
        return matchScore switch
        {
            100 => MatchType.VALID,
            0 => MatchType.INVALID,
            _ => MatchType.NOT_PROCESSED
        };
    }

    private ViesErrorData TryParseError(string content)
    {
        try
        {
            var errorResponse = JsonSerializer.Deserialize<ViesErrorResponse>(content);
            return new ViesErrorData
            {
                ErrorCode = errorResponse?.errorWrapperError?.error?.errorCode,
                ErrorMessage = errorResponse?.errorWrapperError?.error?.errorMessage
            };
        }
        catch
        {
            return new ViesErrorData
            {
                ErrorCode = "PARSE_ERROR",
                ErrorMessage = content
            };
        }
    }

    public async Task<StatusInformationResponse> CheckStatusAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/check-status");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<StatusInformationResponse>(responseContent);
            }
            else
            {
                return new StatusInformationResponse
                {
                    Vow = new VowStatus { Available = false },
                    Countries = new System.Collections.Generic.List<CountryStatus>()
                };
            }
        }
        catch (Exception ex)
        {
            return new StatusInformationResponse
            {
                Vow = new VowStatus { Available = false },
                Countries = new System.Collections.Generic.List<CountryStatus>()
            };
        }
    }

        private async Task<ViesCheckResponse> CheckVatSimpleAsync(
            string targetCountryCode, 
            string targetVatNumber,
            ViesCheckRequest originalRequest)
        {
            try
            {
                string url = $"{_baseUrl}/ms/{targetCountryCode}/vat/{targetVatNumber}";
                
                System.Diagnostics.Debug.WriteLine($"VIES Simple GET Request: {url}");
                
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                
                System.Diagnostics.Debug.WriteLine($"VIES Response Status: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"VIES Response Body: {content}");
                
                if (response.IsSuccessStatusCode)
                {
                    var viesData = JsonSerializer.Deserialize<ViesResponse>(content);
                    return MapViesResponseToVatCheckResponse(viesData, originalRequest);
                }
                else
                {
                    var errorData = TryParseError(content);
                    return new ViesCheckResponse
                    {
                        CountryCode = targetCountryCode,
                        VatNumber = originalRequest.VatNumber,
                        RequestDate = DateTime.Now,
                        Valid = false,
                        HasError = true,
                        ErrorMessage = errorData?.ErrorMessage ?? $"VIES API Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ViesCheckResponse
                {
                    CountryCode = targetCountryCode,
                    VatNumber = originalRequest.VatNumber,
                    RequestDate = DateTime.Now,
                    Valid = false,
                    HasError = true,
                    ErrorMessage = ex.Message
                };
            }
        }
    public class ViesErrorData
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}