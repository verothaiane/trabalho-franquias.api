using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Franquias.Api.Data;
using Franquias.Api.DTOs;

namespace Franquias.Api.Controllers
{
    // Rota base do controller de autenticação
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        // Injeção de dependência do banco de dados e das configurações
        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Endpoint obrigatório
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            // Vai no banco de dados e procura se existe algum usuário com o e-mail digitado
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            // Validação simples: Confere se o usuário existe e se a senha digitada está correta
            if (usuario == null || usuario.SenhaHash != loginDto.Senha)
            {
                // Se errar a senha ou o email, retorna Erro 401 (Não Autorizado)
                return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
            }

            // Regra de negócio: Impede que um usuário inativado faça login no sistema
            if (!usuario.Ativo)
            {
                return Unauthorized(new { mensagem = "Usuário inativo." });
            }

            // Agora vamos gerar o token de acesso
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var keyInfo = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Chave JWT não configurada.");
            var key = Encoding.ASCII.GetBytes(keyInfo);

            // Montando as informações que vão dentro do Token do usuário
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    // Salvamos qual é o perfil dele
                    new Claim(ClaimTypes.Role, usuario.PerfilAcesso.ToString())
                }),
                // Define que o token perde a validade em 2 horas
                Expires = DateTime.UtcNow.AddHours(2), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Cria o token de fato
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Retorna o Status 200 (OK) entregando o Token, o nome e o perfil do usuário logado
            return Ok(new { Token = tokenString, Usuario = usuario.Nome, Perfil = usuario.PerfilAcesso.ToString() });
        }
    }
}