using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Editoras.Interface;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Negocio.Entidades.Editoras;


namespace Biblioteca.Infraestrutura.Dados.Repositorios.Editoras.Implementacoes
{
    public class EditoraRepositorio : EFRepositorioGenerico<Editora>, IEditoraRepositorio
    {
        public EditoraRepositorio(ApplicationDbContext contexto) : base(contexto)
        {
        }
    }
}
