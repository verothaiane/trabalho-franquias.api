namespace Franquias.Api.Models
{
    // Classe responsável por guardar a quantidade de cada produto em uma unidade específica
    public class Estoque
    {
        public int Id { get; set; }
        
        // Relacionamento com a unidade
        public int UnidadeId { get; set; }
        public UnidadeFranqueada? Unidade { get; set; }

        // Relacionamento com o produto
        public int ProdutoId { get; set; }
        public ProdutoServico? Produto { get; set; }

        // Saldo atual do produto na unidade
        public int Quantidade { get; set; }

        // Limite para o relatório de estoque crítico exigido no escopo
        public int QuantidadeMinima { get; set; } 
    }
}