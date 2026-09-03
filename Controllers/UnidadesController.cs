using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Franquias.Api.Data;
using Franquias.Api.Models;

namespace Franquias.Api.Controllers
{
    // Define a rota base deste controller 
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Recebendo o banco de dados por injeção de dependência
        public UnidadesController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint para listar todas as unidades cadastradas (GET)
        [HttpGet]
        public async Task<IActionResult> GetUnidades()
        {
            // Busca todas as unidades no banco de forma assíncrona
            var unidades = await _context.Unidades.ToListAsync();
            return Ok(unidades);
        }

        // Endpoint para cadastrar uma nova unidade (POST)
        [HttpPost]
        public async Task<IActionResult> PostUnidade(UnidadeFranqueada unidade)
        {
            // Verificando regra de negócio: não permitir CNPJ duplicado
            var cnpjExiste = await _context.Unidades.AnyAsync(u => u.Cnpj == unidade.Cnpj);
            if (cnpjExiste)
            {
                // Retorna erro HTTP 400 avisando que o CNPJ já existe
                return BadRequest(new { mensagem = "Já existe uma unidade cadastrada com este CNPJ." });
            }

            // Adiciona a unidade no banco e salva as alterações
            _context.Unidades.Add(unidade);
            await _context.SaveChangesAsync();

            // Retorna status 201 (Created) e mostra os dados cadastrados
            return CreatedAtAction(nameof(GetUnidades), new { id = unidade.Id }, unidade);
        }
    }
}