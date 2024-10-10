using System.Security.Claims;
using System.Security.Cryptography;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Security.Configurations;
using Security.Models;
using Security.Services.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using Models;

namespace Security.Services;

public class LoginService : ILoginService
{
    private const string DateFormat = "yyyy-MM-dd HH:mm:ss";
    private readonly TokenConfiguration _configuration;
    private readonly ITokenService _tokenService;
    private readonly BookstoreReactCoreContext _context;

    public LoginService(TokenConfiguration configuration, ITokenService tokenService, BookstoreReactCoreContext context)
    {
        _configuration = configuration;
        _tokenService = tokenService;
        _context = context;
    }

    public async Task<Token> AuthenticateAsync(Login userCredentials)
    {
        var user = await GetUserByLoginAsync(userCredentials);

        if (user == null) 
            return null;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new Claim(ClaimTypes.Name, user.UserName)
        };
       
        var accessToken = _tokenService.GenerateAccessToken(claims);
        await _context.SaveChangesAsync();

        DateTime createDate = DateTime.Now;
        DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

        return new Token(
            true,
            createDate.ToString(DateFormat),
            expirationDate.ToString(DateFormat),
            accessToken
        );
    }

    public async Task<Token> RefreshTokenAsync(Token token)
    {
        var accessToken = token.AccessToken;

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        
        accessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList());
      
        var createDate = DateTime.Now;
        var expirationDate = createDate.AddMinutes(_configuration.Minutes);

        return new Token(
            true,
            createDate.ToString(DateFormat),
            expirationDate.ToString(DateFormat),
            accessToken
        );
    }

    public async Task<bool> RevokeTokenAsync(string userName)
    {
       return true;
    }

    private async Task<User?> GetUserByLoginAsync(Login user)
    {
        var pass = PasswordHelper.ComputeHash(user.Password, SHA256.Create());
        return await _context.Users.FirstOrDefaultAsync(u => (u.UserName == user.UserName) && (u.Password == pass));
    }

    private async Task<User?> GetUserByUserNameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => (u.UserName == userName));
    }
}

