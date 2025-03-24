using TodoList.Application.Common.Models.Auth;

namespace TodoList.Application.Common.Interfaces;

public interface IAuthService
{
    public Task<AuthModel> RegisterAsync(RegisterModel model);
    public Task<AuthModel> GetTokenAsync(LoginModel model);
}