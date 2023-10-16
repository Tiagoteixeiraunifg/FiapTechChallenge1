using Biblioteca.Infraestrutura.Dados.Contextos;
using Biblioteca.Infraestrutura.Dados.Repositorios.Alunos.Interfaces;
using Biblioteca.Infraestrutura.Dados.Repositorios.Generico;
using Biblioteca.Negocio.Entidades.Alunos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Dados.Repositorios.Alunos.Implementacoes
{
    public class AlunoRepositorioImpl : EFRepositorioGenerico<Aluno>, IAlunoRepositorio
    {
        public AlunoRepositorioImpl(ApplicationDbContext contexto) : base(contexto)
        {
        }

    }
}
