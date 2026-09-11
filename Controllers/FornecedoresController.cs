using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Franquias.Api.Data;
using Franquias.Api.Models;
using Microsoft.AspNetCore.Authorization; 

namespace Franquias.Api.Controllers
{
    // Apenas usuários com tokens podem acessar
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Injeção de dependência no banco de dados
        public FornecedoresController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint para listar todos os fornecedores (GET)
        [HttpGet]
        public async Task<IActionResult> GetFornecedores()
        {
            var fornecedores = await _context.Fornecedores.ToListAsync();
            return Ok(fornecedores);
        }

        // Endpoint para cadastrar um novo fornecedor (POST)
        [HttpPost]
        public async Task<IActionResult> PostFornecedor(Fornecedor fornecedor)
        {
            // não permitir que dois fornecedores tenham o mesmo CNPJ
            var cnpjExiste = await _context.Fornecedores.AnyAsync(f => f.Cnpj == fornecedor.Cnpj);
            if (cnpjExiste)
            {
                return BadRequest(new { mensagem = "Já existe um fornecedor cadastrado com este CNPJ." });
            }

            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();

            // Retorna o Status 201 e os dados salvos
            return CreatedAtAction(nameof(GetFornecedores), new { id = fornecedor.Id }, fornecedor);
        }

        // Endpoint para atualizar os dados de um fornecedor existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFornecedor(int id, Fornecedor fornecedor)
        {
            // Verifica se o ID passado na URL é o mesmo ID do corpo da requisição
            if (id != fornecedor.Id)
            {
                return BadRequest(new { mensagem = "O ID da URL não corresponde ao ID do fornecedor." });
            }

            // Avisa o Entity Framework que este objeto foi modificado e precisa ser salvo
            _context.Entry(fornecedor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Prevenção de erros caso o fornecedor tenha sido deletado no meio tempo
                if (!FornecedorExists(id))
                {
                    return NotFound(new { mensagem = "Fornecedor não encontrado." });
                }
                else
                {
                    throw;
                }
            }

            // Retorna Status 204, que significa sucesso na alteração, mas não há dados novos para mostrar na tela
            return NoContent(); 
        }

        private bool FornecedorExists(int id)
        {
            return _context.Fornecedores.Any(e => e.Id == id);
        }
    }
}