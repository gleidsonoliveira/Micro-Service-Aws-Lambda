namespace SQS.Queue.Queue.Dtos.Response
{
    public class MessageResponse
    {
        public string Atributos { get; set; }

        public string MensagemCorpo { get; set; }

        public string AtributosSistema { get; set; }

        public string MensagemId { get; set; }

        public string NumeroSequencial { get; set; }
    }
}
