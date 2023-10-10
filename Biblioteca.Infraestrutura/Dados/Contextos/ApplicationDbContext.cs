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
        private string CONNECTIONSTRING = "";
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opcoes) : base(opcoes) 
        {
            var infoConexao = ObtenhaDtoInformacoesDeConexaoVariaveisDeAmbiente();
            if (infoConexao.PossuiValor()) 
            {
                CONNECTIONSTRING = infoConexao;
            }
        }

        #region CONFIGURAÇÃO DOS DBSETS

        public DbSet<Usuario> Usuario { get; set; }
        //public DbSet<Pedidos> Pedidos { get; set; }


        #endregion


        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(CONNECTIONSTRING);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }


        /// <summary>
        /// Obtem a connectionstring pelas variáveis de ambiente do sistema
        /// </summary>
        /// <returns></returns>
        private string ObtenhaDtoInformacoesDeConexaoVariaveisDeAmbiente()
        {
            string retorno = "";

            var variaveis = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);

            foreach (DictionaryEntry item in variaveis)
            {
                if (item.Key.Equals("CONNECTIONSTRING"))
                {
                    retorno = (string)item.Value;
                    break;
                }
               
            }

            return retorno;
        }

    }
}
