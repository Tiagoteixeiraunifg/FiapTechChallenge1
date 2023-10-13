using Biblioteca.Negocio.Entidades;
using Biblioteca.Negocio.Entidades.Alunos;
using Biblioteca.Negocio.Utilidades.Conversores;
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

        public AlunoDto()
        {
            this._Conversor = new Conversor<AlunoDto, Aluno>();
        }
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }


        public Aluno ObtenhaEntidade() 
        {
            return _Conversor.ConvertaPara(this);
        }

    }
}
