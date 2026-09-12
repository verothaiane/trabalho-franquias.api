namespace Franquias.Api.Models
{
    // Classe que detalha o que foi vendido dentro de uma venda
    public class ItemVenda
    {
        public int Id { get; set; }
        
        public int VendaId { get; set; }
        
        // Ocultando a venda na hora de gerar o JSON para não criar um loop infinito
        [System.Text.Json.Serialization.JsonIgnore] 
        public Venda? Venda { get; set; }

        // Relacionamento com o produto vendido
        public int ProdutoId { get; set; }
        public ProdutoServico? Produto { get; set; }

        public int Quantidade { get; set; }
        
        // Guardando o preço unitário no momento da venda
        public decimal PrecoUnitario { get; set; }
    }
}