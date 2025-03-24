using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoList.Application.Common.Exceptions;
using TodoList.Application.Common.Interfaces;
using TodoList.Application.Common.Models.Auth;

namespace TodoList.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly JwtConfig _jwt;

    public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtConfig> jwt)
    {
        _userManager = userManager;
        _jwt = jwt.Value;
    }

    public async Task<AuthModel> RegisterAsync(RegisterModel model)
    {
        if (await _userManager.FindByEmailAsync(model.Email) is not null)
        {
            throw new EmailAlreadyExistsException();
        }

        var user = new ApplicationUser
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.Email
        };

        var res = await _userManager.CreateAsync(user, model.Password);

        if (!res.Succeeded)
        {
            return new AuthModel
            {
                IsAuthenticated = false,
                Message = res.Errors.FirstOrDefault()!.Description
            };
        }
        
        var token = await CreateJwtToken(user);

        return new AuthModel
        {
            Email = user.Email,
            ExpiresOn = token.ValidTo,
            IsAuthenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }

    public async Task<AuthModel> GetTokenAsync(LoginModel model)
    {
        var authModel = new AuthModel();
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            throw new PasswordNotCorrectException();
        }
        
        var token = await CreateJwtToken(user);
        authModel.IsAuthenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
        authModel.Email = user.Email;
        authModel.ExpiresOn = token.ValidTo;

        return authModel;
    }

    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);

        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }
}