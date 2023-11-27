using Biblioteca.Negocio.Enumeradores.Validacoes;


namespace Biblioteca.Negocio.Validacoes.FabricaDeValidacoes
{
    public class InconsistenciaDeValidacaoTipado<T> 
    {
        public TipoValidacaoEnum TipoValidacao { get; set; }
        public string? PropriedadeValidada { get; set; }
        public string? Mensagem { get; set; }
        public IList<InconsistenciaDeValidacao> listaDeInconsistencias { get; set; } = new List<InconsistenciaDeValidacao>();
        public IList<string> Mensagens { get; set; } = new List<string>();

        public object _RetornoServico { get; set; }

        public T ObtenhaRetornoServico() 
        {
            T convertido = (T)_RetornoServico;
            return convertido;
        }

        public bool EhValido()
        {
            return listaDeInconsistencias.Count == 0;
        }

        /// <summary>
        /// Merge as validacoes com outra lista de validações
        /// </summary>
        /// <param name="validacoes"></param>
        /// <returns>LIsta de Validações Tipadas</returns>
        public InconsistenciaDeValidacaoTipado<T> MergeValidacoes(InconsistenciaDeValidacaoTipado<T> validacoes) 
        {

            if (this.listaDeInconsistencias.Count <= 0) 
            {
                return validacoes;
            }

            foreach (var inconsistencia in this.listaDeInconsistencias) 
            {
                validacoes.listaDeInconsistencias.Add(inconsistencia);            
            }

            return validacoes;
        }
    }
}
