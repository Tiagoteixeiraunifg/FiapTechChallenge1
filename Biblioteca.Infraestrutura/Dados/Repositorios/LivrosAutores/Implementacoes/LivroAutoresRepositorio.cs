using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Infraestrutura.Dados.Repositorios.LivrosAutores.Interfaces;
using Biblioteca.Negocio.Entidades.Autores;
using Biblioteca.Negocio.Entidades.Livros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.Repositorios.LivrosAutores.Implementacoes
{
    internal class LivroAutoresRepositorio : EFRepositorioGenerico<LivroAutores>, ILivroAutoresRepositorio
    {
        public LivroAutoresRepositorio(ApplicationDbContext contexto) : base(contexto)
        {
        }

        public IList<LivroAutores> ConsultarLivroAutoresPorIdAutor(int id)
        {
            return _DbSet.Where(x => x.AutorId == id).ToList(); 
        }
    }
}
