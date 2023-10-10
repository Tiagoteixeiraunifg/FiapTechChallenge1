using Biblioteca.Infraestrutura.Dados.Contextos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.Configuracao
{

    public static class InjecaoDeConfiguracao
    {
        /// <summary>
        /// Metodo responsável por Adicionar o Contexto e Carregar o Banco de Dados
        /// Inclui classes de Serviço
        /// Inclui classes de Repositorios
        /// </summary>
        public static IServiceCollection AdicioneInfraestrutura(this IServiceCollection services,
           string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext)
                            .Assembly.FullName)));


            return services;
        }
    }
}

