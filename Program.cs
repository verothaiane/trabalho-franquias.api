using Microsoft.EntityFrameworkCore;
using Franquias.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurando a conexão com o banco de dados SQLite local
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//Configuração da Segurança (Autenticação e geração de token JWT)
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!);

// Avisando o sistema a utilização de tokens para proteger a API
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, 
            ValidateAudience = false
        };
    });

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

// Ativando a segurança, onde esses dois comandos controlam a proteção dos endpoints
app.UseAuthentication(); // Quem é o usuário que está tentando acessar
app.UseAuthorization();  // Se o usuário tem permissão para acessar a rota

// Mapeando as rotas da API com base nos Controllers
app.MapControllers();

// Rodando a aplicação
app.Run();