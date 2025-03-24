using Microsoft.AspNetCore.Identity;

namespace TodoList.Application.Common.Interfaces;

public interface IApplicationUser
{ 
    public string Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}