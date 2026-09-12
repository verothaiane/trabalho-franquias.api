namespace Franquias.Api.DTOs
{
    public class GerarRoyaltyDto
    {
        public int UnidadeId { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        
        // Percentual de cobrança.
        public decimal Percentual { get; set; } 
    }
}