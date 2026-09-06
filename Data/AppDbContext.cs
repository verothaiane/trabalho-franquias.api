using Microsoft.EntityFrameworkCore;
using Franquias.Api.Models;

namespace Franquias.Api.Data
{
    // Classe responsável por fazer a ponte entre o código e o banco de dados
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Informando quais classes vão virar tabelas
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UnidadeFranqueada> Unidades { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<ProdutoServico> Produtos { get; set; }

        // Configurações extras das tabelas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Regra de negócio: Garantindo que não existam dois usuários com o mesmo e-mail
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Regra de negócio: Garantindo que não existam duas unidades com o mesmo CNPJ
            modelBuilder.Entity<UnidadeFranqueada>()
                .HasIndex(u => u.Cnpj)
                .IsUnique();

            // Injetando dados de exemplo no banco
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nome = "Administrador Mestre",
                    Email = "admin@franquias.com",
                    SenhaHash = "123456", // Senha simples apenas para fins de teste
                    PerfilAcesso = Perfil.AdministradorFranqueadora, // Perfil de nível máximo
                    Ativo = true,
                    DataCriacao = new DateTime(2026, 9, 4, 12, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}     