using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Franquias.Api.Data;
using Franquias.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Franquias.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EstoquesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstoquesController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint para dar entrada no estoque
        [HttpPost("entrada")]
        public async Task<IActionResult> RegistrarEntrada(int unidadeId, int produtoId, int quantidadeAdicional, int quantidadeMinima = 10)
        {
            // Procura se já existe um registro de estoque para esse produto nessa unidade
            var estoque = await _context.Estoques
                .FirstOrDefaultAsync(e => e.UnidadeId == unidadeId && e.ProdutoId == produtoId);

            if (estoque == null)
            {
                // Se não existir cria o primeiro registro de estoque na prateleira
                estoque = new Estoque 
                {
                    UnidadeId = unidadeId,
                    ProdutoId = produtoId,
                    Quantidade = quantidadeAdicional,
                    QuantidadeMinima = quantidadeMinima // Nível de alerta
                };
                _context.Estoques.Add(estoque);
            }
            else
            {
                // Se já existir, apenas soma a nova quantidade
                estoque.Quantidade += quantidadeAdicional;
                _context.Entry(estoque).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
            return Ok(estoque);
        }

        // Consulta de itens abaixo do estoque mínimo
        [HttpGet("critico")]
        public async Task<IActionResult> GetEstoqueCritico()
        {
            // Busca no banco apenas estoques onde a quantidade atual é menor ou igual ao nível de alerta
            var estoqueCritico = await _context.Estoques
                .Include(e => e.Produto)
                .Include(e => e.Unidade)
                .Where(e => e.Quantidade <= e.QuantidadeMinima)
                .ToListAsync();
            
            return Ok(estoqueCritico);
        }
    }
}