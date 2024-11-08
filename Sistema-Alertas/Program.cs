using Comercial.Api.Services.Jwt;
using Microsoft.EntityFrameworkCore;
using Sistema_Alertas.Database;
using Sistema_Alertas.Endpoints;
using Sistema_Alertas.Extensions;
using Sistema_Alertas.Repository;
using Sistema_Alertas.Services.FileStorage;
using Sistema_Alertas.Services.Jwt;
using Sistema_Alertas.Services.NewFolder;

var builder = WebApplication.CreateBuilder(args);
const string cors = "Cors";

string databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: cors,
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins("*");
            corsPolicyBuilder.AllowAnyMethod();
            corsPolicyBuilder.AllowAnyHeader();
        });
});


builder.Services.AddDbContext<ApplicationDbContext>((options) =>
{
    options.UseNpgsql(databaseConnectionString);
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

builder.Services.AddScoped<IUsuarioRepository, UserRepository>();
builder.Services.AddScoped<IIncidenteRepository, InicidenteRepository>();

builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddSingleton<IFileStorageLocal, FileStorageLocal>();

builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(cors);

//app.UseAuthentication();

app.MapUsuarioEndpoins();

app.MapInicidenteEndpoins();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.ApplyMigrations();

app.MapHub<NotificationHub>("/notifications");

app.Run();

public partial class Program;
