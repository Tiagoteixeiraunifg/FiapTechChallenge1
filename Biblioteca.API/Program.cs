using Biblioteca.Infraestrutura.Dados.Configuracao;
using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Logs.Fabricas;
using Biblioteca.Infraestrutura.Logs.Provedor;
using Biblioteca.Infraestrutura.Seguranca.JWT.Interfaces;
using Biblioteca.Infraestrutura.Seguranca.JWT.Servico;
using Biblioteca.Servicos.Contratos.ContratosDeServicosImplementados;
using Biblioteca.Servicos.Contratos.Servicos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var configuracao = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var secretKey = Encoding.ASCII.GetBytes(configuracao.GetValue<string>("Secret"));


// Add services to the container.
builder.Services.AddControllers();


//Adicionando Contexto de Conexão e Banco de Dados
builder.Services.AdicioneInfraestrutura(configuracao);
builder.Services.AddScoped<IServicoUsuario, ServicoUsuarioImpl>();
builder.Services.AddScoped<IServicoAluno, ServicoAlunoImpl>();

builder.Services.AddScoped<IServicoAutor, ServicoAutor>();
builder.Services.AddScoped<IServicoEditora, ServicoEditora>();


//Parametrização do Swagger
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var nomeXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var caminhoArquivo = Path.Combine(AppContext.BaseDirectory, nomeXml);


    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Biblioteca API", Version = "v1.0" });

    c.IncludeXmlComments(caminhoArquivo);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
            "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n\r\n" +
            "Exemplo (informar sem as aspas): 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


//inserindo Provedor Padrão de Logs
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new ProvedorDeLogsCustomizado(new FabricaDeLogs()));




//Configuração de Segurança
builder.Services.AddScoped<IServicoDeToken, ServicoDeToken>();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(builder => builder.AllowAnyMethod()
                              .AllowAnyOrigin()
                              .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();