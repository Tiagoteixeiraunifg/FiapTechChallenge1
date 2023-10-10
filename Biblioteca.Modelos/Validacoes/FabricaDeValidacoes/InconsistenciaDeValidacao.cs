using Biblioteca.Negocio.Enumeradores.Validacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Validacoes.FabricaDeValidacoes
{
    public class InconsistenciaDeValidacao
    {
        public TipoValidacaoEnum TipoValidacao { get; set; }
        public string? PropriedadeValidada { get; set; }
        public string? Mensagem { get; set; }
        public IList<InconsistenciaDeValidacao> listaDeInconsistencias { get; set; } = new List<InconsistenciaDeValidacao>();
        public IList<string> Mensagens { get; set; } = new List<string>();

        public bool EhValido()
        {
            return listaDeInconsistencias.Count == 0;
        }
    }
}
