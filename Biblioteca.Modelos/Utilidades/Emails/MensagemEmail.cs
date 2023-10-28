using Biblioteca.Negocio.Enumeradores.Emails;


namespace Biblioteca.Negocio.Utilidades.Emails
{
    public class MensagemEmail
    {
        public TipoEmailEnum TipoEmail { get; set; }

        public TipoMensagemEmailEnum TipoDeMensagem { get; set; }

        public DestinatariosEmail DestinatariosEmail { get; set; }

        public AnexosEmail AnexosEmail { get; set; }

        public string TextoMensagemEmail { get; set; }

        public string AssuntoEmail { get; set; }
    }
}
