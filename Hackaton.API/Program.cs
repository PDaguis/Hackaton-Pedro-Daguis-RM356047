using Hackaton.API.Extensions;
using Hackaton.API.Logging;
using Hackaton.API.Security;
using Hackaton.Core.Enumerators;
using Hackaton.Core.Interfaces;
using Hackaton.Infra.Context;
using Hackaton.Infra.Mappings;
using Hackaton.Infra.Repositories;
using Hackaton.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("All", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration() { LogLevel = LogLevel.Information }));

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
MongoDbClassMapping.RegisterClassMaps();

builder.Services.AddSecurity(configuration);

builder.Services.Configure<HackatonOptions>(opt => configuration.GetSection("DatabaseConfiguration").Bind(opt));

builder.Services.AddScoped<ApplicationDbContext>();

builder.Services.AddScoped<TokenProvider>();

builder.Services.AddDependencyInjections();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("All");

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
