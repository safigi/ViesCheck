namespace ViesApi;

public interface IViesApiService
{
    Task<ViesCheckResponse> CheckVatNumberAsync(ViesCheckRequest request);
    Task<StatusInformationResponse> CheckStatusAsync();
}