using Microsoft.EntityFrameworkCore;
using LouvorApp.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura a API para ser visível na rede local (Ouvir em todas as interfaces)
builder.WebHost.UseUrls("http://*:5000"); 

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura a política de liberação de acesso (CORS)
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build(); // A variável 'app' nasce aqui!

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// OBRIGATÓRIO: UseCors deve vir antes de MapControllers
app.UseCors(); 

app.UseAuthorization();
app.MapControllers();

app.Run();