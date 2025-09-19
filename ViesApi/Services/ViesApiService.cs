using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ViesApi.Configuration;
using ViesApi.Interfaces;
using ViesApi.Models;
using MatchType = ViesApi.Models.MatchType;

namespace ViesApi.Services;

public class ViesApiService : IViesApiService
{
    private readonly string _baseUrl;
    private readonly HttpClient _httpClient;
    private readonly ILogger<ViesApiService> _logger;

    public ViesApiService(IHttpClientFactory httpClientFactory, ILogger<ViesApiService> logger, ViesApiConfiguration config)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
        _baseUrl = config.BaseUrl;
        
        _httpClient.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", config.UserAgent);
    }

    public async Task<ViesCheckResponse> CheckVatNumberAsync(ViesCheckRequest request)
    {
        _logger.LogInformation("Starting VAT validation for {CountryCode}-{VatNumber}", request.CountryCode, request.VatNumber);
        
        try
        {
            if (!string.IsNullOrEmpty(request.RequesterMemberStateCode) &&
                !string.IsNullOrEmpty(request.RequesterNumber))
            {
                return await CheckVatWithRequesterAsync(
                    request.CountryCode,
                    request.VatNumber,
                    request.RequesterMemberStateCode,
                    request.RequesterNumber,
                    request);
            }

            return await CheckVatSimpleAsync(
                request.CountryCode,
                request.VatNumber,
                request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "VAT validation failed for {CountryCode}-{VatNumber}", request.CountryCode, request.VatNumber);
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

    public async Task<StatusInformationResponse> CheckStatusAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/check-status");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<StatusInformationResponse>(responseContent)!;
            }

            return new StatusInformationResponse
            {
                Vow = new VowStatus { Available = false },
                Countries = []
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "VIES status check failed");
            return new StatusInformationResponse
            {
                Vow = new VowStatus { Available = false },
                Countries = []
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
        var url = $"{_baseUrl}/ms/{targetCountryCode}/vat/{targetVatNumber}" +
                  $"?requesterMemberStateCode={requesterCountryCode}&requesterNumber={requesterVatNumber}";
        
        try
        {
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var viesData = JsonSerializer.Deserialize<ViesResponse>(content);
                var result = MapViesResponseToVatCheckResponse(viesData, originalRequest);
                _logger.LogInformation("VAT validation completed for {CountryCode}-{VatNumber}: {Valid}", 
                    targetCountryCode, targetVatNumber, result.Valid);
                return result;
            }

            var errorData = TryParseError(content);
            _logger.LogWarning("VIES API error for {CountryCode}-{VatNumber}: {ErrorMessage}", 
                targetCountryCode, targetVatNumber, errorData.ErrorMessage);
            
            return new ViesCheckResponse
            {
                CountryCode = targetCountryCode,
                VatNumber = originalRequest.VatNumber,
                RequestDate = DateTime.Now,
                Valid = false,
                HasError = true,
                ErrorMessage = errorData.ErrorMessage ?? $"VIES API Error: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception during VAT validation for {CountryCode}-{VatNumber}", 
                targetCountryCode, targetVatNumber);
            
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

    private static ViesCheckResponse MapViesResponseToVatCheckResponse(ViesResponse? viesData,
        ViesCheckRequest originalRequest)
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
            Name = CleanViesValue(viesData.name)!,
            Address = CleanViesValue(viesData.address)!,
            HasError = false,
            ErrorMessage = null,

            TraderNameMatch = MatchType.NOT_PROCESSED,
            TraderStreetMatch = MatchType.NOT_PROCESSED,
            TraderPostalCodeMatch = MatchType.NOT_PROCESSED,
            TraderCityMatch = MatchType.NOT_PROCESSED,
            TraderCompanyTypeMatch = MatchType.NOT_PROCESSED
        };

        if (viesData.viesApproximate == null) return response;
        response.TraderNameMatch = ConvertMatchScore(viesData.viesApproximate.matchName);
        response.TraderStreetMatch = ConvertMatchScore(viesData.viesApproximate.matchStreet);
        response.TraderPostalCodeMatch = ConvertMatchScore(viesData.viesApproximate.matchPostalCode);
        response.TraderCityMatch = ConvertMatchScore(viesData.viesApproximate.matchCity);
        response.TraderCompanyTypeMatch = ConvertMatchScore(viesData.viesApproximate.matchCompanyType);

        return response;
    }

    private static DateTime ParseRequestDate(string requestDate)
    {
        return DateTime.TryParse(requestDate, out var result) ? result : DateTime.Now;
    }

    private static string? CleanViesValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value == "---")
            return null;
        return value.Trim();
    }

    private static MatchType ConvertMatchScore(int matchScore)
    {
        return matchScore switch
        {
            100 => MatchType.VALID,
            0 => MatchType.INVALID,
            _ => MatchType.NOT_PROCESSED
        };
    }

    private static ViesErrorData TryParseError(string content)
    {
        try
        {
            var errorResponse = JsonSerializer.Deserialize<ViesErrorResponse>(content);
            return new ViesErrorData
            {
                ErrorCode = errorResponse?.errorWrapperError.error.errorCode,
                ErrorMessage = errorResponse?.errorWrapperError.error.errorMessage
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

    private async Task<ViesCheckResponse> CheckVatSimpleAsync(
        string targetCountryCode,
        string targetVatNumber,
        ViesCheckRequest originalRequest)
    {
        var url = $"{_baseUrl}/ms/{targetCountryCode}/vat/{targetVatNumber}";
        
        try
        {
            Debug.WriteLine($"VIES Simple GET Request: {url}");

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            Debug.WriteLine($"VIES Response Status: {response.StatusCode}");
            Debug.WriteLine($"VIES Response Body: {content}");

            if (response.IsSuccessStatusCode)
            {
                var viesData = JsonSerializer.Deserialize<ViesResponse>(content);
                var result = MapViesResponseToVatCheckResponse(viesData, originalRequest);
                _logger.LogInformation("VAT validation completed for {CountryCode}-{VatNumber}: {Valid}", 
                    targetCountryCode, targetVatNumber, result.Valid);
                return result;
            }

            var errorData = TryParseError(content);
            _logger.LogWarning("VIES API error for {CountryCode}-{VatNumber}: {ErrorMessage}", 
                targetCountryCode, targetVatNumber, errorData.ErrorMessage);
            
            return new ViesCheckResponse
            {
                CountryCode = targetCountryCode,
                VatNumber = originalRequest.VatNumber,
                RequestDate = DateTime.Now,
                Valid = false,
                HasError = true,
                ErrorMessage = errorData.ErrorMessage ?? $"VIES API Error: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception during VAT validation for {CountryCode}-{VatNumber}", 
                targetCountryCode, targetVatNumber);
            
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
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
