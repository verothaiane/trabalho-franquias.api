using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Franquias.Api.Data;
using Microsoft.AspNetCore.Authorization;

namespace Franquias.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RelatoriosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RelatoriosController(AppDbContext context)
        {
            _context = context;
        }

        // Faturamento por unidade e período
        [HttpGet("faturamento")]
        public async Task<IActionResult> GetFaturamento(int unidadeId, DateTime dataInicio, DateTime dataFim)
        {
            var faturamento = await _context.Vendas
                .Where(v => v.UnidadeId == unidadeId && v.DataVenda >= dataInicio && v.DataVenda <= dataFim)
                .SumAsync(v => v.ValorTotal);

            return Ok(new { UnidadeId = unidadeId, DataInicio = dataInicio, DataFim = dataFim, TotalFaturamento = faturamento });
        }

        // Ranking de unidades por faturamento
        [HttpGet("ranking-unidades")]
        public async Task<IActionResult> GetRankingUnidades(DateTime dataInicio, DateTime dataFim)
        {
            var ranking = await _context.Vendas
                .Where(v => v.DataVenda >= dataInicio && v.DataVenda <= dataFim)
                .GroupBy(v => new { v.UnidadeId, v.Unidade.Nome }) // Agrupa as vendas de cada franquia
                .Select(g => new 
                {
                    UnidadeId = g.Key.UnidadeId,
                    NomeUnidade = g.Key.Nome,
                    TotalFaturamento = g.Sum(v => v.ValorTotal) // Soma tudo que a franquia vendeu
                })
                .OrderByDescending(r => r.TotalFaturamento) // Coloca quem vendeu mais no topo
                .ToListAsync();

            return Ok(ranking);
        }

        // Produtos mais vendidos
        [HttpGet("produtos-mais-vendidos")]
        public async Task<IActionResult> GetProdutosMaisVendidos()
        {
            var rankingProdutos = await _context.ItensVenda
                .GroupBy(i => new { i.ProdutoId, i.Produto.Nome })
                .Select(g => new 
                {
                    ProdutoId = g.Key.ProdutoId,
                    NomeProduto = g.Key.Nome,
                    QuantidadeTotalVendida = g.Sum(i => i.Quantidade)
                })
                .OrderByDescending(p => p.QuantidadeTotalVendida)
                .Take(10) // Retorna apenas o Top 10
                .ToListAsync();

            return Ok(rankingProdutos);
        }

        // Total de royalties gerados
        [HttpGet("royalties-gerados")]
        public async Task<IActionResult> GetTotalRoyalties(int mes, int ano)
        {
            var totalRoyalties = await _context.Royalties
                .Where(r => r.Mes == mes && r.Ano == ano)
                .SumAsync(r => r.ValorDevido);

            return Ok(new { Mes = mes, Ano = ano, TotalRoyalties = totalRoyalties });
        }
    }
}