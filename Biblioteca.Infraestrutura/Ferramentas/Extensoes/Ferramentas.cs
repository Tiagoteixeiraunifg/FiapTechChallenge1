using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Ferramentas.Extensoes
{
    public static class Ferramentas
    {
        /// <summary>
        /// Método de extensão responsavel por verificar se uma string possui valor.
        /// </summary>
        /// <param name="value">O objeto a ser validado.</param>
        /// <returns>
        /// Verdadeiro ou falso de acordo com a validação.
        /// </returns>
        public static bool PossuiValor(this string value)
        {
            return PossuiValor(value, true);
        }

        /// <summary>
        /// Método de extensão responsavel por verificar se uma string possui valor.
        /// </summary>
        /// <param name="value">O objeto a ser validado.</param>
        /// <param name="considerarEspacosComoValor">Informação se a verificação deve considerar espaço em branco como valor.</param>
        /// <returns>
        /// Verdadeiro ou falso de acordo com a validação.
        /// </returns>
        public static bool PossuiValor(this string value, bool considerarEspacosComoValor)
        {
            return considerarEspacosComoValor ? !string.IsNullOrEmpty(value) : !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Método de extensão responsavel por verificar se um interiro possui valor.
        /// </summary>
        /// <param name="value">O objeto a ser validado.</param>
        /// <returns>
        /// Verdadeiro ou falso de acordo com a validação.
        /// </returns>
        public static bool PossuiValor(this int value)
        {
            return value != 0;
        }

        /// <summary>
        /// Método de extensão responsavel por verificar se um interiro possui valor.
        /// </summary>
        /// <param name="value">O objeto a ser validado.</param>
        /// <returns>
        /// Verdadeiro ou falso de acordo com a validação.
        /// </returns>
        public static bool PossuiValor(this int? value)
        {
            return value != null && value != 0;
        }

        /// <summary>
        /// Método de extensão responsavel por verificar se uma string possui valor.
        /// </summary>
        /// <param name="value">O objeto a ser validado.</param>
        /// <returns>
        /// Verdadeiro ou falso de acordo com a validação.
        /// </returns>
        public static bool PossuiValor(this Guid value)
        {
            return value != null && value != new Guid() && value != Guid.Empty;
        }

        /// <summary>
        /// Método de extensão responsavel por verificar se uma data possui valor.
        /// </summary>
        /// <param name="value">O objeto a ser validado.</param>
        /// <returns>
        /// Verdadeiro ou falso de acordo com a validação.
        /// </returns>
        public static bool PossuiValor(this DateTime? value)
        {
            return value.GetValueOrDefault() != DateTime.MinValue;
        }

        /// <summary>
        /// Método de extensão responsavel por verificar se uma data possui valor.
        /// </summary>
        /// <param name="value">O objeto a ser validado.</param>
        /// <returns>
        /// Verdadeiro ou falso de acordo com a validação.
        /// </returns>
        public static bool PossuiValor(this DateTime value)
        {
            return value == null ? false : value != DateTime.MinValue;
        }


        /// <summary>
        /// Método de extensão responsavel por verificar se um objeto possui valor ou não.
        /// </summary>
        /// <param name="value">O objeto a ser validado.</param>
        /// <returns>
        /// Verdadeiro ou falso de acordo com a validação.
        /// </returns>
        public static bool PossuiValor(this object value)
        {
            return value != null;
        }

        /// <summary>
        /// Método de extensão responsavel por verificar se uma lista declarada pela interface possui valor ou não.
        /// </summary>
        /// <typeparam name="T">O tipo genérico da lista.</typeparam>
        /// <param name="value">A lista a ser validada.</param>
        /// <returns>
        /// Verdadeiro ou falso de acordo com a validação.
        /// </returns>
        public static bool PossuiValor<T>(this IList<T> value)
        {
            return value != null && value.Count > 0;
        }

        /// <summary>
        /// Método de extensão responsavel por verificar se um variavel do tipo float possui valor.
        /// </summary>
        /// <param name="value">O objeto a ser validado.</param>
        /// <returns>
        /// Verdadeiro ou falso de acordo com a validação.
        /// </returns>
        public static bool PossuiValor(this float value)
        {
            return value != 0;
        }

        /// <summary>
        /// Método de extensão responsavel por verificar se um variavel do tipo float possui valor.
        /// </summary>
        /// <param name="value">O objeto a ser validado.</param>
        /// <returns>
        /// Verdadeiro ou falso de acordo com a validação.
        /// </returns>
        public static bool PossuiValor(this float? value)
        {
            return value == null ? false : value != 0;
        }


        /// <summary>
        /// Método de extensão responsavel por verificar se um variavel do tipo float possui valor.
        /// </summary>
        /// <param name="value">O objeto a ser validado.</param>
        /// <returns>
        /// Verdadeiro ou falso de acordo com a validação.
        /// </returns>
        public static bool PossuiValor(this decimal value)
        {
            return value != 0;
        }

        /// <summary>
        /// Método de extensão responsavel por verificar se um variavel do tipo float possui valor.
        /// </summary>
        /// <param name="value">O objeto a ser validado.</param>
        /// <returns>
        /// Verdadeiro ou falso de acordo com a validação.
        /// </returns>
        public static bool PossuiValor(this decimal? value)
        {
            return value == null ? false : value != 0;
        }

        /// <summary>
        /// Método de extensão responsável por verificar se a datatable contém linhas
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool PossuiLinhas(this DataTable value)
        {
            return value.Rows.Count == 0 ? false : value.Rows.Count != 0;
        }

        /// <summary>
        /// Método de extensão responsável por verificar se a Lista contém linhas
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool PossuiLinhas<T>(this IList<T> value)
        {
            return value.Count == 0 ? false : value.Count != 0;
        }

        /// <summary>
        /// Faz a conversão do valor passado para um booleano.
        /// </summary>
        /// <param name="valor">O valor para conversão.</param>
        /// <returns>O boolenao que representa o valor passado.</returns>
        public static bool? ObtenhaBooleano(object valor)
        {
            var valorString = valor as string;

            if (string.IsNullOrEmpty(valorString))
            {
                return null;
            }

            return valorString.ToUpper(CultureInfo.InvariantCulture) == "SIM";
        }



        /// <summary>
        /// Retorna a quantidade de minutos passados em relação a data atual.
        /// </summary>
        /// <param name="data">A data de referência.</param>
        /// <returns>A quantidade de minutos executados.</returns>
        public static double ObtenhaMinutosExecutados(this DateTime data)
        {
            var dataAtual = DateTime.Now;
            var diferenca = dataAtual.Subtract(data);

            return diferenca.TotalMinutes;
        }

        /// <summary>
        /// Método de extensão para obter e converter o valor de um enumerador para string.
        /// </summary>
        /// <param name="valor">O item do enumerador.</param>
        /// <returns>O valor do enumerador convertido.</returns>
        public static string ObtenhaValor(this Enum valor)
        {
            return Convert.ToInt16(valor, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Método de extensão para obter e converter o valor de um enumerador para string.
        /// </summary>
        /// <param name="valor">O item do enumerador.</param>
        /// <returns>O valor do enumerador convertido.</returns>
        public static string ObtenhaValorEnum(this Enum valor)
        {
            return valor == null ? string.Empty : Convert.ToInt16(valor, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Método de extensão para obter a descrição do enumerador.
        /// </summary>
        /// <param name="valor">O item do enumerador valor.</param>
        /// <returns>A descrição do enumerador.</returns>
        public static string ObtenhaDescricao(this Enum valor)
        {
            return valor.ToString();
        }

        public static string DuasCasasSemSifra(this decimal valor)
        {
            return string.Format("{0:C2}", valor).Replace("R$", "");
        }

        public static string QuatroCasasSemSifra(this decimal valor)
        {
            return string.Format("{0:C4}", valor).Replace("R$", "");
        }

        public static string DuasCasasComSifra(this decimal valor)
        {
            return string.Format("{0:C2}", valor);
        }

        public static string QuatroCasasComSifra(this decimal valor)
        {
            return string.Format("{0:C4}", valor);
        }

        public static decimal TruncaDecimal(this decimal valor)
        {
            return Math.Truncate(valor);
        }

        public static decimal ArredondeDecimal(this decimal valor, int CasasDecimais)
        {
            return Math.Round(valor, CasasDecimais);
        }

        public static decimal ParaDecimalDuasCasas(this string valor)
        {
            return valor == null || valor.Length == 0 ? 0 : Decimal.Parse(valor).ArredondeDecimal(2);
        }

        public static DateTime AdicioneDiasCarencia(this DateTime date, object valor)
        {
            return date.AddDays(float.Parse(valor.ToString()));
        }

    }
}
