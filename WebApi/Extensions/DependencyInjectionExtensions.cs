using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Common;
using Domain.Context;
using Domain.Ports;
using Domain.Services;
using Domain.Services.Abstracts;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Validations;

namespace WebApi.Extensions;

public static class DependencyInjectionExtensions
{
    public static void ConfigureDb(this IServiceCollection service, WebApplicationBuilder builder)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        service.AddDbContext<JwtContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("WebApi")).UseSnakeCaseNamingConvention());
    }

    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<IPasswordHasher, PasswordHasherService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailSenderService, EmailSenderService>();
    }

    public static void AddFluentValidation(this IServiceCollection service)
    {
        service.AddTransient<NewAccountViewModelValidate>();
    }

    public static void AddJwtTokenAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = AuthOptions.Issuer,
                    ValidAudience = AuthOptions.Audience,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
                };
            });
    }

    public static void ConfigureSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API"); });
    }

    public static void ConfigureJsonOptions(this JsonSerializerOptions options)
    {
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    }
}