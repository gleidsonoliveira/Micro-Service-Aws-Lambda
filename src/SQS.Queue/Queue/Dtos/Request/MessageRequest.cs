namespace SQS.Queue.Queue.Dtos.Request
{
    public class MessageRequest
    {
        public List<string> NomeAtributos { get; set; }

        public int MaximoMensagens { get; set; }

        public List<string> MensagemNomeAtributos { get; set; }

        public string FilaUrl { get; set; }

        public string TentativasRequestAtendida { get; set; }

        public int EsperaSegundos { get; set; }

        public int VisibilidadeTimeOut { get; set; }
    }
}
