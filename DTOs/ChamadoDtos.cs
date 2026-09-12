namespace Franquias.Api.DTOs
{
    public class NovoChamadoDto
    {
        public int UnidadeId { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public string Prioridade { get; set; } = "Média";
        public string Descricao { get; set; } = string.Empty;
    }

    public class AtualizarChamadoDto
    {
        public string Status { get; set; } = string.Empty;
        public string ComentarioAtualizacao { get; set; } = string.Empty;
    }
}