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

        var user = _mapper.Map<ApplicationUser>(model);

        await _userManager.CreateAsync(user, model.Password);
        var token = await CreateJwtToken(user);

        return new AuthModel
        {
            Email = user.Email,
            ExpiresOn = token.ValidTo,
            IsAuthenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }
    
    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);

        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
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