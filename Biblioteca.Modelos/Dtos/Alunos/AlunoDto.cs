using Biblioteca.Negocio.Entidades;
using Biblioteca.Negocio.Entidades.Alunos;
using Biblioteca.Negocio.Utilidades.Conversores;
using Biblioteca.Negocio.Validacoes.Alunos;
using Biblioteca.Negocio.Validacoes.FabricaDeValidacoes;
using NPOI.OpenXmlFormats.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Dtos.Alunos
{
    [Serializable]
    public class AlunoDto : EntidadeBase
    {
        private Conversor<AlunoDto, Aluno> _Conversor;

        private AlunoValidador _Validador;

        private InconsistenciaDeValidacao _InconsistenciaDeValidacao;

        public AlunoDto()
        {
            this._Conversor = new Conversor<AlunoDto, Aluno>();
            this._Validador = new AlunoValidador();
        }


        public string Nome { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }


        public bool EhValidoAtualizacao() 
        {
            _InconsistenciaDeValidacao = _Validador.ValideAtualizacaoDeAluno(this.ObtenhaEntidade());
            return _InconsistenciaDeValidacao.EhValido();
        }

        public bool EhValidoCadastro()
        {
            _InconsistenciaDeValidacao = _Validador.ValideCadastroDeAluno(this.ObtenhaEntidade());
            return _InconsistenciaDeValidacao.EhValido();

        }

        public InconsistenciaDeValidacao RetornarInconsistencia() => _InconsistenciaDeValidacao;

        public Aluno ObtenhaEntidade() => _Conversor.ConvertaPara(this);
        

    }
}
