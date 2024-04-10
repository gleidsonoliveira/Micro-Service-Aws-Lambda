using Amazon.SQS.Model;
using SQS.Queue.Queue.Dtos.Request;
using SQS.Queue.Queue.Dtos.Response;

namespace SQS.Queue.Queue.Interface
{
    internal interface ISqsService
    {
        Task<MessageResponse> SendFileAsyncMessage<T>(string filaUrl, T request);

        Task<List<MessageTratadaResponse<T>>>FetchMessageGetsAsyncDeleted<T>(MessageRequest request);

        Task<List<MessageTratadaResponse<T>>> FetchFileAsyncMessage<T>(MessageRequest request);

        Task DeleteAsyncMessage(string QueueUrl, string Id);
        /// <summary>
        /// Desrealiza Mensagem
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mensagem"></param>
        /// <returns></returns>
        T UnrealizeMessage<T>(Message mensagem);
    }
}
