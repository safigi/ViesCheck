using ViesApi.Models;

namespace ViesApi.Interfaces;

public interface IViesApiService
{
    Task<ViesCheckResponse> CheckVatNumberAsync(ViesCheckRequest request);
    Task<StatusInformationResponse> CheckStatusAsync();
}