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
    public class AlunoDto : EntidadeBase
    {
        private Conversor<AlunoDto, Aluno> _Conversor;

        public AlunoDto(Conversor<AlunoDto, Aluno> conversor)
        {
            this._Conversor = conversor;
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
