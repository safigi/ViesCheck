namespace ViesApi.Models;

public class CommonResponse
{
    public bool ActionSucceed { get; set; }
    public List<ErrorWrapper> ErrorWrappers { get; set; }
}