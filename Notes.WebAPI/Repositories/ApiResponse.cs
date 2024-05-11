using System.Net;

namespace Notes.WebAPI.Repositories;

public class ApiResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
}
