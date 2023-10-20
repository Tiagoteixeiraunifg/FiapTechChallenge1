using Biblioteca.Negocio.Enumeradores.Validacoes;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Validacoes.FabricaDeValidacoes
{
    public abstract class ValidadorAbstratro<T> : AbstractValidator<T>, IValidador<T>
    {

        private IList<InconsistenciaDeValidacao> _inconsistencias = new List<InconsistenciaDeValidacao>();

        private InconsistenciaDeValidacao _inconsistencia = new InconsistenciaDeValidacao();

        private InconsistenciaDeValidacaoTipado<T> _inconsistenciaTipada = new InconsistenciaDeValidacaoTipado<T>();

        private ValidationResult resultado = new ValidationResult();



        public InconsistenciaDeValidacao Valide(T t)
        {
            resultado = base.Validate(t);

            if (resultado.IsValid) { return new InconsistenciaDeValidacao(); }

            _inconsistencia.listaDeInconsistencias = new List<InconsistenciaDeValidacao>();

            foreach (ValidationFailure item in resultado.Errors)
            {
                _inconsistencia.listaDeInconsistencias.Add(
                    new InconsistenciaDeValidacao
                    {
                        TipoValidacao = item.Severity == Severity.Error ? TipoValidacaoEnum.IMPEDITIVA : TipoValidacaoEnum.AVISO,
                        PropriedadeValidada = ObtenhaValorDaPropriedade(item.FormattedMessagePlaceholderValues),
                        Mensagem = item.ErrorMessage,

                    });
            }

            return _inconsistencia;
        }

        private string ObtenhaValorDaPropriedade(Dictionary<string, object> dicionario)
        {
            string valorDaPropriedade = "";
            foreach (KeyValuePair<string, object> item in dicionario)
            {
                if (item.Key.ToString().Equals("PropertyName"))
                {
                    valorDaPropriedade = item.Value.ToString();
                    break;
                }
            }
            return valorDaPropriedade;
        }


        public IList<InconsistenciaDeValidacao> ValideLista(T t)
        {
            resultado = base.Validate(t);
            if (resultado.IsValid) { return null; }

            if (_inconsistencias.Count > 0) { _inconsistencias.Clear(); }

            foreach (ValidationFailure item in resultado.Errors)
            {
                _inconsistencias.Add(
                    new InconsistenciaDeValidacao
                    {
                        TipoValidacao = (TipoValidacaoEnum)(int)item.Severity,
                        PropriedadeValidada = item.PropertyName,
                        Mensagem = item.ErrorMessage,

                    });
            }

            return _inconsistencias;
        }

        public bool Valido(T t)
        {
            return Valide(t).EhValido();
        }

        public bool ExisteInconsistencias()
        {
            return !resultado.IsValid;
        }

        public InconsistenciaDeValidacaoTipado<T> ValideTipado(T t)
        {
            resultado = base.Validate(t);

            if (resultado.IsValid) { return new InconsistenciaDeValidacaoTipado<T>(); }

            _inconsistenciaTipada.listaDeInconsistencias = new List<InconsistenciaDeValidacao>();

            foreach (ValidationFailure item in resultado.Errors)
            {
                _inconsistencia.listaDeInconsistencias.Add(
                    new InconsistenciaDeValidacao
                    {
                        TipoValidacao = item.Severity == Severity.Error ? TipoValidacaoEnum.IMPEDITIVA : TipoValidacaoEnum.AVISO,
                        PropriedadeValidada = ObtenhaValorDaPropriedade(item.FormattedMessagePlaceholderValues),
                        Mensagem = item.ErrorMessage,

                    });
            }

            return _inconsistenciaTipada;
        }
    }
}
