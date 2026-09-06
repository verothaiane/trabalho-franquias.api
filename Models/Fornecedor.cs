namespace Franquias.Api.Models
{
    // Classe que representa as empresas que fornecem os produtos para a franquia
    public class Fornecedor
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string Contato { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }
}