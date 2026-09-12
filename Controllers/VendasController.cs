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
    public class VendasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VendasController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint para registrar uma venda e baixar o estoque
        [HttpPost]
        public async Task<IActionResult> RegistrarVenda(NovaVendaDto dto)
        {
            // Uma venda deverá possuir pelo menos um item
            if (dto.Itens == null || !dto.Itens.Any())
                return BadRequest(new { mensagem = "A venda precisa ter pelo menos um item." });

            var unidade = await _context.Unidades.FindAsync(dto.UnidadeId);
            if (unidade == null) return NotFound(new { mensagem = "Unidade não encontrada." });

            // Uma unidade inativa não poderá registrar novas vendas
            if (!unidade.Ativo)
                return BadRequest(new { mensagem = "Unidades inativas não podem registrar vendas." });

            var novaVenda = new Venda 
            { 
                UnidadeId = dto.UnidadeId, 
                ValorTotal = 0, // Vai começar zerado e somar sozinho abaixo
                DataVenda = DateTime.UtcNow
            };

            // Processa cada produto que está sendo vendido
            foreach (var itemDto in dto.Itens)
            {
                var produto = await _context.Produtos.FindAsync(itemDto.ProdutoId);
                if (produto == null) return NotFound(new { mensagem = $"Produto ID {itemDto.ProdutoId} não encontrado." });

                // Busca o estoque deste produto específico nesta unidade
                var estoque = await _context.Estoques
                    .FirstOrDefaultAsync(e => e.UnidadeId == dto.UnidadeId && e.ProdutoId == itemDto.ProdutoId);

                // O estoque não poderá ficar negativo após uma venda
                if (estoque == null || estoque.Quantidade < itemDto.Quantidade)
                    return BadRequest(new { mensagem = $"Estoque insuficiente para o produto {produto.Nome}. Saldo atual: {(estoque?.Quantidade ?? 0)}" });

                // O valor total da venda deverá ser calculado a partir dos itens, quantidades e preços
                var valorItem = produto.PrecoBase * itemDto.Quantidade;
                novaVenda.ValorTotal += valorItem;

                // Atualização do estoque após confirmação da venda
                estoque.Quantidade -= itemDto.Quantidade;
                _context.Entry(estoque).State = EntityState.Modified; // Avisa o banco que o saldo mudou

                // Adiciona o item na nota fiscal
                novaVenda.Itens.Add(new ItemVenda
                {
                    ProdutoId = itemDto.ProdutoId,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.PrecoBase
                });
            }

            // Salva a venda e os novos saldos de estoque tudo de uma vez
            _context.Vendas.Add(novaVenda);
            await _context.SaveChangesAsync();

            // Retorna a venda concluída
            return CreatedAtAction(nameof(GetVenda), new { id = novaVenda.Id }, novaVenda);
        }

        // Endpoint para consultar uma venda específica 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVenda(int id)
        {
            var venda = await _context.Vendas
                .Include(v => v.Itens) // Traz os itens da venda
                .ThenInclude(i => i.Produto) // Traz os dados do produto dentro do item
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venda == null) return NotFound();
            return Ok(venda);
        }
    }
}