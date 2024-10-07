using System.Security.Claims;
using System.Text;
using DAL.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Security.Configurations;
using Security.Services;
using Security.Services.Interfaces;
using Services;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Token Configuration
var tokenConfigurations = new TokenConfiguration();
new ConfigureFromConfigurationOptions<TokenConfiguration>(builder.Configuration.GetSection("TokenConfigurations")).Configure(tokenConfigurations);
builder.Services.AddSingleton(tokenConfigurations);

// ADD authentication with bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = tokenConfigurations.Issuer,
        ValidAudience = tokenConfigurations.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret)),
        NameClaimType = JwtRegisteredClaimNames.UniqueName
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var userRepository = context.HttpContext.RequestServices.GetRequiredService<ILoginService>();
            var claimsPrincipal = context.Principal;

            var userName = context.Principal.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                context.Fail("Invalid token: User not found in token.");
                return;
            }

            var user = await userRepository.GetUserByUserNameAsync(userName);
            if (user == null)
            {
                context.Fail("Invalid token: User does not exist.");
                return;
            }

            var refreshTokenClaim = claimsPrincipal.FindFirst("RefreshToken")?.Value;
            if (refreshTokenClaim != user.RefreshToken)
            {
                context.Fail("Invalid token: Refresh token does not match.");
                return;
            }

            // TOKEN VALID
        }
    };
});

// Add authorization
builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());
});

// Add CORS
builder.Services.AddCors(options =>
{
    // To allow all routes
    //options.AddDefaultPolicy(policy =>
    //{
    //    policy.AllowAnyOrigin()
    //        .AllowAnyHeader()
    //        .AllowAnyMethod();
    //});

    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<BookstoreReactCoreContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Services injection
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Build app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseCors(); // For all routes
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
