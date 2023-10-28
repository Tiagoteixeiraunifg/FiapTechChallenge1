using System.Net.Mail;


namespace Biblioteca.Negocio.Utilidades.Emails
{
    public class DestinatariosEmail
    {
        /// <summary>
        /// Lista com os destinatários do email
        /// </summary>
        public List<MailAddress> Destinatarios { get; set; }

        /// <summary>
        /// Lista com os destinatarios do email cc (com cópia)
        /// </summary>
        public List<MailAddress> Destinatarios_cc { get; set; }

        /// <summary>
        /// Lista com os destinatarios do email cco (com cópia oculta)
        /// </summary>
        public List<MailAddress> Destinatarios_cco { get; set; }

    }
}
