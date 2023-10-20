using Biblioteca.Negocio.Enumeradores.Validacoes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio.Validacoes.FabricaDeValidacoes
{
    /// <summary>
    /// Extensões do FluentValidation.
    /// </summary>
    public static class Extensoes
    {
        /// <summary>
        /// Sobrescreve a propriedade validada caso o parâmetro seja diferente de nulo/vazio.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto validado.</typeparam>
        /// <typeparam name="TProperty">Propriedade validada.</typeparam>
        /// <param name="regra">Regra da validação.</param>
        /// <param name="propriedade">Nome da propriedade validada.</param>
        /// <returns>Regra da validação com o tipo definido.</returns>
        public static IRuleBuilderOptions<T, TProperty> SobrescrevaPropriedade<T, TProperty>(this IRuleBuilderOptions<T, TProperty> regra, string propriedade) where T : class
        {
            return string.IsNullOrWhiteSpace(propriedade) ? regra : regra.OverridePropertyName(propriedade);
        }

        /// <summary>
        /// Define o tipo de validação.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto validado.</typeparam>
        /// <typeparam name="TProperty">Propriedade validada.</typeparam>
        /// <param name="regra">Regra da validação.</param>
        /// <param name="tipoDaValidacao">Tipo de validação.</param>
        /// <returns>Regra da validação com o tipo definido.</returns>
        public static IRuleBuilderOptions<T, TProperty> TipoValidacao<T, TProperty>(this IRuleBuilderOptions<T, TProperty> regra, TipoValidacaoEnum tipoDaValidacao) where T : class
        {
            switch (tipoDaValidacao)
            {
                case TipoValidacaoEnum.IMPEDITIVA:
                    return regra.WithSeverity(Severity.Error);
                case TipoValidacaoEnum.ADVERTENCIA:
                    return regra.WithSeverity(Severity.Warning);
                case TipoValidacaoEnum.AVISO:
                    return regra.WithSeverity(Severity.Info);
                default:
                    return regra.WithSeverity(Severity.Error);

            }
        }


    }
}
