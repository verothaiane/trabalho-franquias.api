using System;

namespace Franquias.Api.Models
{
    // Classe que representa a tabela de usuários no banco de dados
    public class Usuario
    {
        public int Id { get; set; } // Chave primária automática
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; // Será usado para o login
        public string SenhaHash { get; set; } = string.Empty; // Senha criptografada por segurança
        public Perfil PerfilAcesso { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}