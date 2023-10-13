using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.ServicoDados.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.ServicoDados.Implementacoes
{
    public class DataService : IDataService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DataService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void InicializarBd()
        {
            _applicationDbContext.Database.EnsureCreated();
        }
    }
}
