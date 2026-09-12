namespace Franquias.Api.Models
{
    // Classe que representa a cobrança mensal da franquia
    public class Royalty
    {
        public int Id { get; set; }
        
        public int UnidadeId { get; set; }
        public UnidadeFranqueada? Unidade { get; set; }
        
        public int Mes { get; set; }
        public int Ano { get; set; }
        
        public decimal ValorFaturamento { get; set; } // A soma de todas as vendas do mês
        public decimal PercentualAplicado { get; set; } 
        public decimal ValorDevido { get; set; } // Faturamento multiplicado pelo percentual
        
        public bool Pago { get; set; } = false; // Controle de pagamento
        public DateTime DataCalculo { get; set; } = DateTime.UtcNow;
    }
}