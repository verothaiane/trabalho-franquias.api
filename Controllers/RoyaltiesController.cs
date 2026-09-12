using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Franquias.Api.Data;
using Franquias.Api.Models;
using Franquias.Api.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Franquias.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoyaltiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoyaltiesController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint que calcula a taxa automaticamente buscando as vendas do mês
        [HttpPost("calcular")]
        public async Task<IActionResult> CalcularRoyalty(GerarRoyaltyDto dto)
        {
            // Verifica se a unidade existe
            var unidade = await _context.Unidades.FindAsync(dto.UnidadeId);
            if (unidade == null) return NotFound(new { mensagem = "Unidade não encontrada." });

            // Busca todas as vendas desta unidade no mês e ano solicitados
            var vendasPeriodo = await _context.Vendas
                .Where(v => v.UnidadeId == dto.UnidadeId && 
                            v.DataVenda.Month == dto.Mes && 
                            v.DataVenda.Year == dto.Ano)
                .ToListAsync();

            // Soma o valor de todas as vendas para encontrar o faturamento do mês
            var faturamento = vendasPeriodo.Sum(v => v.ValorTotal);

            // Cria o registro da cobrança
            var royalty = new Royalty
            {
                UnidadeId = dto.UnidadeId,
                Mes = dto.Mes,
                Ano = dto.Ano,
                ValorFaturamento = faturamento,
                PercentualAplicado = dto.Percentual,
                ValorDevido = faturamento * dto.Percentual,
                Pago = false
            };

            _context.Royalties.Add(royalty);
            await _context.SaveChangesAsync();

            return Ok(royalty);
        }

        // Endpoint para marcar a cobrança como paga
        [HttpPut("{id}/pagar")]
        public async Task<IActionResult> PagarRoyalty(int id)
        {
            var royalty = await _context.Royalties.FindAsync(id);
            if (royalty == null) return NotFound(new { mensagem = "Cobrança não encontrada." });

            royalty.Pago = true;
            _context.Entry(royalty).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Royalty marcado como pago com sucesso!", royalty });
        }
    }
}