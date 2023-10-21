using Biblioteca.Negocio.Entidades.FichaEmprestimos;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Validacoes.FichaEmprestimoAlunos
{
    public class FichaEmprestimoAlunoValidador : ValidadorAbstratro<FichaEmprestimoAluno>
    {

        public FichaEmprestimoAlunoValidador()
        {
            
        }


        public InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> ValideCadastroFicha(FichaEmprestimoAluno dados) 
        {

            AssineRegrasDeCadastroDaFicha();
            return base.ValideTipado(dados);
        }

        public InconsistenciaDeValidacaoTipado<FichaEmprestimoAluno> ValideFinalizacaoFicha(FichaEmprestimoAluno dados)
        {


            return base.ValideTipado(dados);
        }



        private void AssineRegrasDeCadastroDaFicha() 
        {

        }






    }
}
