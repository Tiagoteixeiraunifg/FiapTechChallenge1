using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Alunos.Implementacoes;
using Biblioteca.Infraestrutura.Dados.Repositorios.Alunos.Interfaces;
using Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Implementacoes;
using Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Interfaces;
using Biblioteca.Infraestrutura.Dados.ServicoDados.Implementacoes;
using Biblioteca.Infraestrutura.Dados.ServicoDados.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Biblioteca.Infraestrutura.Dados.Configuracao
{

    public static class InjecaoDeConfiguracao
    {
        /// <summary>
        /// Metodo responsável por Adicionar o Contexto e Carregar o Banco de Dados
        /// Inclui classes de Serviço
        /// Inclui classes de Repositorios
        /// </summary>
        public static IServiceCollection AdicioneInfraestrutura(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorioImpl>();
            services.AddScoped<IAlunoRepositorio, AlunoRepositorioImpl>();
            
            services.AddTransient<IDataService, DataService>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString")) , ServiceLifetime.Scoped); 
                

         

            services.BuildServiceProvider().GetService<IDataService>().InicializarBd();
      
            return services;
        }
    }
}

