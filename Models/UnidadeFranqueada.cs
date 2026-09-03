using System;

namespace Franquias.Api.Models
{
    // Classe que representa as franquias cadastradas no sistema
    public class UnidadeFranqueada
    {
        public int Id { get; set; } // Chave primária automática
        public string Nome { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string Responsavel { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Contato { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public bool Ativa { get; set; } = true; // Define se a unidade está operando ou não
    }
}