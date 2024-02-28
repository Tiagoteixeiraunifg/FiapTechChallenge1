﻿using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Alunos.Implementacoes;
using Biblioteca.Infraestrutura.Dados.Repositorios.Alunos.Interfaces;
using Biblioteca.Infraestrutura.Dados.Repositorios.Autores.Implementacoes;
using Biblioteca.Infraestrutura.Dados.Repositorios.Autores.Interface;
using Biblioteca.Infraestrutura.Dados.Repositorios.Editoras.Implementacoes;
using Biblioteca.Infraestrutura.Dados.Repositorios.Editoras.Interface;
using Biblioteca.Infraestrutura.Dados.Repositorios.FichaEmprestimoAlunos.Implementacoes;
using Biblioteca.Infraestrutura.Dados.Repositorios.FichaEmprestimoAlunos.Interfaces;
using Biblioteca.Infraestrutura.Dados.Repositorios.Livros.Implementacoes;
using Biblioteca.Infraestrutura.Dados.Repositorios.Livros.Interfaces;
using Biblioteca.Infraestrutura.Dados.Repositorios.LivrosAutores.Implementacoes;
using Biblioteca.Infraestrutura.Dados.Repositorios.LivrosAutores.Interfaces;
using Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Implementacoes;
using Biblioteca.Infraestrutura.Dados.Repositorios.Usuarios.Interfaces;
using Biblioteca.Infraestrutura.Dados.ServicoDados.Implementacoes;
using Biblioteca.Infraestrutura.Dados.ServicoDados.Interfaces;
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
        public static IServiceCollection AdicioneInfraestrutura(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorioImpl>();
            services.AddScoped<IAlunoRepositorio, AlunoRepositorioImpl>();
            services.AddScoped<IEditoraRepositorio, EditoraRepositorio>();
            services.AddScoped<IAutorRepositorio, AutorRepositorio>();
            services.AddScoped<ILivroRepositorio, LivroRepositorio>();
            services.AddScoped<ILivroAutoresRepositorio, LivroAutoresRepositorio>();
            services.AddScoped<IFichaEmprestimoAlunoRepositorio, FichaEmprestimoAlunoRepositorioImpl>();
            services.AddTransient<IDataService, DataService>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString")) , ServiceLifetime.Scoped);




            services.BuildServiceProvider().GetService<IDataService>().InicializarBd();
      
            return services;
        }

        public static IServiceCollection AdicioneInfraestruturav2(this IServiceCollection services, string connectionString)
        {

            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorioImpl>();
            services.AddScoped<IAlunoRepositorio, AlunoRepositorioImpl>();
            services.AddScoped<IEditoraRepositorio, EditoraRepositorio>();
            services.AddScoped<IAutorRepositorio, AutorRepositorio>();
            services.AddScoped<ILivroRepositorio, LivroRepositorio>();
            services.AddScoped<ILivroAutoresRepositorio, LivroAutoresRepositorio>();
            services.AddScoped<IFichaEmprestimoAlunoRepositorio, FichaEmprestimoAlunoRepositorioImpl>();
            services.AddTransient<IDataService, DataService>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString), ServiceLifetime.Scoped);




            //services.BuildServiceProvider().GetService<IDataService>().InicializarBd();

            return services;
        }
    }
}

