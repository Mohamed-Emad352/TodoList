namespace TodoList.Application.Common.Models.Auth;

public class AuthModel
{
    public string Message { get; set; } = string.Empty;
    public bool IsAuthenticated { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresOn { get; set; }
}
