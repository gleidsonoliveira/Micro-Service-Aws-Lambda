namespace SQS.Queue.Queue.Dtos.Response
{
    public class MessageTratadaResponse<T>
    {
        /// <summary>
        /// Recibo
        /// </summary>
        public string ReceiptHandle { get; set; }
        /// <summary>
        /// Id da mensagem
        /// </summary>
        public string MessageId { get; set; }
        /// <summary>
        /// mensagem
        /// </summary>
        public T Message { get; set; }
    }
}
