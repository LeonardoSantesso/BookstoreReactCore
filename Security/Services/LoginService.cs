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

    public async Task<Token> ValidateCredentialsAsync(Login userCredentials)
    {
        var user = await GetUserByLoginAsync(userCredentials);

        if (user == null) 
            return null;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var refreshToken = _tokenService.GenerateRefreshToken();

        claims.Add(new Claim("RefreshToken", refreshToken));

        var accessToken = _tokenService.GenerateAccessToken(claims);
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configuration.DaysToExpiry);

        await _context.SaveChangesAsync();

        DateTime createDate = DateTime.Now;
        DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

        return new Token(
            true,
            createDate.ToString(DateFormat),
            expirationDate.ToString(DateFormat),
            accessToken,
            refreshToken
        );
    }

    public async Task<Token> ValidateCredentialsAsync(Token token)
    {
        var accessToken = token.AccessToken;
        var refreshToken = token.RefreshToken;

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

        var username = principal.Identity.Name;

        var user = await GetUserByUserNameAsync(username);

        if (user == null ||
            user.RefreshToken != refreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now) return null;

        accessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList());
        refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;

        await _context.SaveChangesAsync();

        var createDate = DateTime.Now;
        var expirationDate = createDate.AddMinutes(_configuration.Minutes);

        return new Token(
            true,
            createDate.ToString(DateFormat),
            expirationDate.ToString(DateFormat),
            accessToken,
            refreshToken
        );
    }

    public async Task<bool> RevokeTokenAsync(string userName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => (u.UserName == userName));
        
        if (user is null) 
            return false;

        user.RefreshToken = null;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User?> GetUserByLoginAsync(Login user)
    {
        var pass = PasswordHelper.ComputeHash(user.Password, SHA256.Create());
        return await _context.Users.FirstOrDefaultAsync(u => (u.UserName == user.UserName) && (u.Password == pass));
    }

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => (u.UserName == userName));
    }
}

