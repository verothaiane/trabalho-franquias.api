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
        }
    }
}