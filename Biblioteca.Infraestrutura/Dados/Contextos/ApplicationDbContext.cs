using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Entidades.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.Contextos
{
    public class ApplicationDbContext : DbContext
    {

        private static ApplicationDbContext _ApplicationDbContext;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opcoes) : base(opcoes) 
        {
            _ApplicationDbContext = this;
        }

        #region CONFIGURAÇÃO DOS DBSETS

        public DbSet<Usuario> Usuario { get; set; }
        //public DbSet<Pedidos> Pedidos { get; set; }


        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
        
        public static ApplicationDbContext Instancia() 
        {
            return _ApplicationDbContext;
        }

    }
}
