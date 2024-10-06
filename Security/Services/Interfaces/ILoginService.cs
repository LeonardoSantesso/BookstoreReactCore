using Models;
using Security.Models;

namespace Security.Services.Interfaces;

public interface ILoginService
{
    Task<Token> ValidateCredentialsAsync(Login userCredentials);
    Task<Token> ValidateCredentialsAsync(Token token);
    Task<bool> RevokeTokenAsync(string userName);
    Task<User?> GetUserByLoginAsync(Login user);
    Task<User?> GetUserByUserNameAsync(string userName);
}