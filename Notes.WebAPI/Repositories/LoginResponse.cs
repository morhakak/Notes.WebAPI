namespace Notes.WebAPI.Repositories;

public class LoginResponse : ApiResponse
{
    public string Token { get; set; } = string.Empty;
}
