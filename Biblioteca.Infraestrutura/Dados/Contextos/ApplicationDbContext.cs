using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Negocio.Entidades.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.Contextos
{
    public class ApplicationDbContext : DbContext
    {

        private static ApplicationDbContext _ApplicationDbContext;
        private static DbContextOptions<ApplicationDbContext> _opcoes;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opcoes) : base(opcoes) 
        {
            _ApplicationDbContext = this;
            _opcoes = opcoes;   
        }

        #region CONFIGURAÇÃO DOS DBSETS

        //public DbSet<Usuario> Usuario { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
        

        public static ApplicationDbContext NovaInstancia() 
        {
            return new ApplicationDbContext(_opcoes);
        }

        public static ApplicationDbContext Instancia()
        {
            var DbContextOpt = new DbContextOptionsBuilder<ApplicationDbContext>();
            var stringconexao = Environment.GetEnvironmentVariable("CONNECTIONSTRING");
            DbContextOpt.UseSqlServer(stringconexao);
            var opcoesnovas = DbContextOpt.Options;
            _opcoes = opcoesnovas;

            return  new ApplicationDbContext(_opcoes);
        }
    }
}
