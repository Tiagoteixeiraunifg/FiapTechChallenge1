using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.FichaEmprestimoAlunos.Interfaces;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.Repositorios.FichaEmprestimoAlunos.Implementacoes
{
    public class FichaEmprestimoAlunoRepositorioImpl : EFRepositorioGenerico<FichaEmprestimoAluno>, IFichaEmprestimoAlunoRepositorio
    {
        public FichaEmprestimoAlunoRepositorioImpl(ApplicationDbContext contexto) : base(contexto)
        {
        }
    }
}
