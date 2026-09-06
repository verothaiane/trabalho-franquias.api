namespace Franquias.Api.Models
{
    // Classe que representa o catálogo padronizado pela rede de franquias
    public class ProdutoServico
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty; 
        
        // Usando números decimais para trabalhar com dinheiro de forma mais precisa
        public decimal PrecoBase { get; set; } 
        public bool Ativo { get; set; } = true;

        // Relacionamento: Um produto pode ter um fornecedor associado
        public int? FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }
    }
}