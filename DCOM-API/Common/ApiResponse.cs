using Microsoft.AspNetCore.SignalR.Protocol;

namespace DCOM_API.Common;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public List<ApiMessage> Messages { get; set; } = new();
    public string ResponseType { get; set; } = "Success";

    public static ApiResponse<T> Success(T data, string? message = null)
    {
        var response = new ApiResponse<T> { Data = data, ResponseType = "Success" };
        if (!string.IsNullOrWhiteSpace(message))
            response.Messages.Add(new ApiMessage { Text = message });
        return response;
    }

    public static ApiResponse<T> Fail(string message, string? code = null) =>
        new()
        {
            ResponseType = "Error",
            Messages = { new ApiMessage { Text = message, Code = code } }
        };
}