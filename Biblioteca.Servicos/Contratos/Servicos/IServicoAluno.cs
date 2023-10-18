using Biblioteca.Negocio.Entidades.Alunos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Servicos.Contratos.Servicos
{
    public interface IServicoAluno
    {
        IList<Aluno> ObtenhaTodosAlunos();

        Aluno ObtenhaAluno(int Id);

        Aluno CadastreAluno(Aluno dto);

        Aluno AtualizeAluno(Aluno dto);

        List<string> DeleteAluno(int Id);
    }
}
