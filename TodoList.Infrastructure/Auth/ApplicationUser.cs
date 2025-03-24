using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TodoList.Application.Common.Interfaces;
using TodoList.Application.Common.Models.Auth;

namespace TodoList.Infrastructure.Auth;

public class ApplicationUser : IdentityUser, IApplicationUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<RegisterModel, ApplicationUser>();
        }
    }
}
