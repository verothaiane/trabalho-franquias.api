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
    public class ChamadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChamadosController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint para listar os chamados em aberto por prioridade
        [HttpGet]
        public async Task<IActionResult> GetChamados([FromQuery] string? status)
        {
            var query = _context.Chamados.Include(c => c.Unidade).AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(c => c.Status == status);
            }

            // Ordena pela data de abertura (os mais antigos primeiro)
            var chamados = await query.OrderBy(c => c.DataAbertura).ToListAsync();
            return Ok(chamados);
        }

        // Endpoint para a unidade abrir um chamado
        [HttpPost]
        public async Task<IActionResult> AbrirChamado(NovoChamadoDto dto)
        {
            var unidade = await _context.Unidades.FindAsync(dto.UnidadeId);
            if (unidade == null) return NotFound(new { mensagem = "Unidade não encontrada." });

            var chamado = new ChamadoSuporte
            {
                UnidadeId = dto.UnidadeId,
                Categoria = dto.Categoria,
                Prioridade = dto.Prioridade,
                Descricao = dto.Descricao,
                Status = "Aberto",
                DataAbertura = DateTime.UtcNow
            };

            _context.Chamados.Add(chamado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChamados), new { id = chamado.Id }, chamado);
        }

        // Endpoint para registrar atualização e encerramento
        [HttpPut("{id}/atualizar")]
        public async Task<IActionResult> AtualizarChamado(int id, AtualizarChamadoDto dto)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado == null) return NotFound(new { mensagem = "Chamado não encontrado." });

            chamado.Status = dto.Status;
            
            // Adiciona o novo comentário ao histórico, quebrando a linha
            chamado.HistoricoAtualizacao += $"\n[{DateTime.UtcNow:dd/MM/yyyy HH:mm}] {dto.ComentarioAtualizacao}";

            // Se o status for "encerrado", grava a data de encerramento automaticamente
            if (dto.Status.ToUpper() == "ENCERRADO")
            {
                chamado.DataEncerramento = DateTime.UtcNow;
            }

            _context.Entry(chamado).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(chamado);
        }
    }
}