namespace Franquias.Api.DTOs
{
    //Usando um DTO para receber somente o necessário no swagger
    public class NovaVendaDto
    {
        public int UnidadeId { get; set; }
        
        // Uma lista contendo os produtos que o cliente está comprando
        public List<NovoItemVendaDto> Itens { get; set; } = new List<NovoItemVendaDto>();
    }

    public class NovoItemVendaDto
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}