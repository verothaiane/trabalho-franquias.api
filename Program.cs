using Microsoft.EntityFrameworkCore;
using Franquias.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurando a conexão com o banco de dados SQLite local
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionando suporte para Controllers na API
builder.Services.AddControllers();

// Configurações do Swagger para documentar a API e facilitar os testes
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ativando o Swagger quando estiver em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapeando as rotas da API com base nos Controllers
app.MapControllers();

// Rodando a aplicação
app.Run();