using Biblioteca.Negocio.Dtos.Livros;
using Biblioteca.Negocio.Entidades.Livros;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Servicos.Contratos.Servicos
{
    public interface IServicoLivro
    {
        IList<Livro> ObtenhaTodosLivros();

        Livro ObtenhaLivro(int Id);

        InconsistenciaDeValidacaoTipado<Livro> Cadastrar(LivroDto livro);

        InconsistenciaDeValidacaoTipado<Livro> Atualizar(LivroDto livro);

        InconsistenciaDeValidacaoTipado<Livro> Deletar(int Id);



    }
}
