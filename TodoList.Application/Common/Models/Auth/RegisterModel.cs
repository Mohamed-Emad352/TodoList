using TodoList.Application.Common.Interfaces;

namespace TodoList.Application.Common.Models.Auth;

public class RegisterModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}