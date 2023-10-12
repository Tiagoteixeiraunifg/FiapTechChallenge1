using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Implementacoes;
using Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Interfaces;
using Biblioteca.Servicos.Contratos.ContratosDeServicosImplementados;
using Biblioteca.Servicos.Contratos.Servicos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


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
           IConfiguration configuration)
        {
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorioImpl>();
            services.AddScoped<IServicoUsuario, ServicoUsuarioImpl>();
           

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString")), ServiceLifetime.Scoped);



            return services;
        }
    }
}

