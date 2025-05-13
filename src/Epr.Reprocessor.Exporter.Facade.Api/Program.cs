using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Epr.Reprocessor.Exporter.Facade.Api.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;

namespace Epr.Reprocessor.Exporter.Facade.Api;

[ExcludeFromCodeCoverage]
public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services
            .AddApplicationInsightsTelemetry()
            .AddHealthChecks();

        // Logging
        builder.Services.AddLogging();

        // Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options);
            }, options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options);
            });

        // Authorization
        var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        builder.Services.AddAuthorizationBuilder().AddPolicy("AuthUser", policy);

        // General Config
        builder.Services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            options.OperationFilter<AddAuthHeaderOperationFilter>();
        });

        // App
        var app = builder.Build();
        app.UseExceptionHandler(app.Environment.IsDevelopment() ? "/error-development" : "/error");
        app.UseSwagger();
        app.UseSwaggerUI();

        if (app.Environment.IsDevelopment())
        {
            IdentityModelEventSource.ShowPII = true;
        }
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        await app.RunAsync();
    }
}