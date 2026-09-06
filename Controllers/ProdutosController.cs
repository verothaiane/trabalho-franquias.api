using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Franquias.Api.Data;
using Franquias.Api.Models;
using Microsoft.AspNetCore.Authorization; 

namespace Franquias.Api.Controllers
{
    // Somente usuários logados tem acesso
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint para listar e filtrar produtos
        // Usando FromQuery para permitir que o usuário passe filtros na URL do Swagger
        [HttpGet]
        public async Task<IActionResult> GetProdutos([FromQuery] string? categoria, [FromQuery] string? nome, [FromQuery] bool? ativo)
        {
            // Inicia a busca no banco
            var query = _context.Produtos.Include(p => p.Fornecedor).AsQueryable();

            // Se o usuário digitou uma categoria é filtrado por ela
            if (!string.IsNullOrEmpty(categoria))
            {
                query = query.Where(p => p.Categoria.Contains(categoria));
            }

            // Se digitou um nome também é filtrado
            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(p => p.Nome.Contains(nome));
            }

            // Se passou o status (ativo/inativo), o filtro é aplicado
            if (ativo.HasValue)
            {
                query = query.Where(p => p.Ativo == ativo.Value);
            }

            var produtos = await query.ToListAsync();
            return Ok(produtos);
        }

        // Endpoint para cadastrar um novo produto (POST)
        [HttpPost]
        public async Task<IActionResult> PostProduto(ProdutoServico produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProdutos), new { id = produto.Id }, produto);
        }
    }
}