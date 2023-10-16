using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Autores.Interface;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Negocio.Entidades.Autores;
using Biblioteca.Negocio.Entidades.Editoras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.Repositorios.Autores.Implementacoes
{
    internal class AutorRepositorio : EFRepositorioGenerico<Autor>, IAutorRepositorio
    {
        public AutorRepositorio(ApplicationDbContext contexto) : base(contexto)
        {
        }
    }
}
