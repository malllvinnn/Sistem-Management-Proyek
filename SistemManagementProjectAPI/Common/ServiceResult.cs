namespace SistemManagementProjectAPI.Common;

public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new List<string>();

    public static ServiceResult<T> Success(T data, string message = "Success")
    {
        var serviceResult = new ServiceResult<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };

        return serviceResult;
    }

    public static ServiceResult<T> Failure(string message, List<string>? errors = null)
    {
        var serviceResult = new ServiceResult<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };

        return serviceResult;
    }
}