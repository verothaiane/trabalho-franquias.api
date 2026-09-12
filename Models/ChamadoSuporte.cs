namespace Franquias.Api.Models
{
    // Entidade que representa os tickets de suporte
    public class ChamadoSuporte
    {
        public int Id { get; set; }
        
        public int UnidadeId { get; set; }
        public UnidadeFranqueada? Unidade { get; set; }
        
        public string Categoria { get; set; } = string.Empty; // Ex: Sistema, Financeiro, Dúvida
        public string Prioridade { get; set; } = string.Empty; // Ex: Baixa, Média, Alta
        public string Descricao { get; set; } = string.Empty;
        public string Status { get; set; } = "Aberto"; // Pode ser: Aberto, Em Andamento, Encerrado
        
        // Campo para registrar as respostas e atualizações da franqueadora
        public string? HistoricoAtualizacao { get; set; } 
        
        public DateTime DataAbertura { get; set; } = DateTime.UtcNow;
        public DateTime? DataEncerramento { get; set; }
    }
}