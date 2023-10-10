using Biblioteca.Infraestrutura.Dados.Configuracao;
using Biblioteca.Infraestrutura.Dados.Contextos;

var builder = WebApplication.CreateBuilder(args);
var configuracao = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

// Add services to the container.

builder.Services.AddControllers();




builder.Services.AdicioneInfraestrutura(configuracao.GetValue<string>("ConnectionString"));
builder.Services.AddDbContext<ApplicationDbContext>(ServiceLifetime.Scoped);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
