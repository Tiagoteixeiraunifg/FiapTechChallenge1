using Biblioteca.Infraestrutura.Dados.Configuracao;
using Biblioteca.Servicos.Contratos.ContratosDeServicosImplementados;
using Biblioteca.Servicos.Contratos.Servicos;
using Biblioteca.Servicos.Notificacoes.Emails;
using Biblioteca.WorkerService.FluxoFicha;
using Biblioteca.WorkerService.FluxoFicha.Eventos;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var configuracao = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var conexaoServiceBus = configuracao.GetSection("MassTransitAzure")["Conexao"] ?? Environment.GetEnvironmentVariable("SERVICE_BUS_CONEXAO");
        var fila = configuracao.GetSection("MassTransitAzure")["Fila"] ?? Environment.GetEnvironmentVariable("SERVICE_BUS_NOME_FILA");
        var connectionString = configuracao.GetSection("ConnectionStrings")["ConnectionString"] ?? Environment.GetEnvironmentVariable("CONNECTIONSTRING");

        services.AddHostedService<Worker>();

        services.AdicioneInfraestruturav2(connectionString);

        services.AddMassTransit(x =>
        {
            x.UsingAzureServiceBus((contexto, config) =>
            {
                config.Host(conexaoServiceBus);

                config.ReceiveEndpoint(fila, e =>

                e.Consumer<EventoFichaEmprestimoConsumidor>()
               
                );
            });
        });

        services.AddScoped<IServicoUsuario, ServicoUsuarioImpl>();
        services.AddScoped<IServicoAluno, ServicoAlunoImpl>();
        services.AddScoped<IServicoLivro, ServicoLivroImpl>();
        services.AddScoped<IServicoAutor, ServicoAutor>();
        services.AddScoped<IServicoEditora, ServicoEditora>();
        services.AddScoped<IServicoFichaEmprestimoAluno, ServicoFichaEmprestimoAlunoImpl>();
        services.InicieServicoNotificacao(configuracao);


    })
    .Build();

host.Run();
