using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GrpcDotNet.Services;
using Auth;
using System.Security.Claims;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddGrpc(opt =>
{
});

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt => opt.TokenValidationParameters= new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateActor = false,
        ValidateLifetime = false,
        IssuerSigningKey = JwtHelper.SecurityKey
    });

services.AddAuthorization(opt => opt.AddPolicy(JwtBearerDefaults.AuthenticationScheme, p =>
{
    p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    p.RequireClaim(ClaimTypes.Name);
}));
services.AddGrpcHealthChecks(o =>
{
}).AddCheck("My cool service", () => HealthCheckResult.Healthy(), new[] {"grpc", "live"});
 
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<FirstService>();
app.MapGrpcHealthChecksService();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

public partial class Program { }