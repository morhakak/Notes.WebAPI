using System.Net;

namespace Notes.WebAPI.Repositories;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}
