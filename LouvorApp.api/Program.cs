using Microsoft.EntityFrameworkCore;
using LouvorApp.Api.Data; // <-- Se faltar essa linha, ele não acha o AppDbContext

var builder = WebApplication.CreateBuilder(args);

// 1. Configurando o Banco de Dados (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Adicionando os Controllers e o Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Configurando o Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();