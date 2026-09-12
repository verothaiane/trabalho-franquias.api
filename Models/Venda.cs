namespace Franquias.Api.Models
{
    // Classe que representa a nota fiscal da venda, vinculada a uma unidade
    public class Venda
    {
        public int Id { get; set; }
        
        public int UnidadeId { get; set; }
        public UnidadeFranqueada? Unidade { get; set; }

        public DateTime DataVenda { get; set; } = DateTime.UtcNow;

        // O valor total será calculado automaticamente somando os itens
        public decimal ValorTotal { get; set; }

        // Uma venda possui uma lista de vários itens dentro dela
        public List<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
    }
}