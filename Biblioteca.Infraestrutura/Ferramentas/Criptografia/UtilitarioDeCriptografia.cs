using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Infraestrutura.Ferramentas.Criptografia
{
    public class UtilitarioDeCriptografia
    {
        /// <summary>
        /// Representação de valor em base 64 (Chave Interna).
        /// </summary>
        private const string CRYPTOKEY = "jesuseosenhordossenhores";

        /// <summary>
        /// Vetor de bytes utilizados para a criptografia (Chave Externa).
        /// </summary>
        private static byte[] bIV = { 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18, 0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC };

        /// <summary>
        /// Metodo de criptografia de valor.
        /// </summary>
        /// <param name="text">Valor a ser criptografado.</param>
        /// <returns>
        /// Valor criptografado.
        /// </returns>
        public static string Criptografe(string text)
        {
            // Se a string não está vazia, executa a criptografia
            if (!string.IsNullOrEmpty(text))
            {
                // Cria instancias de vetores de bytes com as chaves
                var bKey = Convert.FromBase64String(CRYPTOKEY);
                var bText = new UTF8Encoding().GetBytes(text);

                Rijndael rijndael = new RijndaelManaged();
                rijndael.KeySize = 256;

                MemoryStream mStream = new MemoryStream();
                CryptoStream encryptor = new CryptoStream(mStream, rijndael.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write);
                encryptor.Write(bText, 0, bText.Length);
                encryptor.FlushFinalBlock();

                return Convert.ToBase64String(mStream.ToArray());
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Metodo de descriptografia.
        /// </summary>
        /// <param name="text">O texto criptografado.</param>
        /// <returns>
        /// Valor descriptografado.
        /// </returns>
        public static string Descriptografe(string text)
        {
            // Se a string não está vazia, executa a criptografia
            if (!string.IsNullOrEmpty(text))
            {
                var bKey = Convert.FromBase64String(CRYPTOKEY);
                var bText = Convert.FromBase64String(text);

                Rijndael rijndael = new RijndaelManaged();
                rijndael.KeySize = 256;

                MemoryStream mStream = new MemoryStream();
                CryptoStream decryptor =
                new CryptoStream(mStream, rijndael.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write);
                decryptor.Write(bText, 0, bText.Length);
                decryptor.FlushFinalBlock();

                // Instancia a classe de codificação para que a string venha de forma correta
                UTF8Encoding utf8 = new UTF8Encoding();
                return utf8.GetString(mStream.ToArray());
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
