using Hackaton.Core.Enumerators;
using Hackaton.Core.Interfaces;
using Hackaton.Infra.Context;
using Hackaton.Infra.Repositories;
using Hackaton.Shared.Options;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

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

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

//ConventionRegistry.Register("EnumStringConvention", new ConventionPack { new EnumRepresentationConvention(BsonType.String) }, t => true);
//BsonSerializer.RegisterSerializer(new EnumStringSerializer<EEspecialidade>());

builder.Services.Configure<HackatonOptions>(opt => configuration.GetSection("DatabaseConfiguration").Bind(opt));

builder.Services.AddScoped<ApplicationDbContext>();

builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("All");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
