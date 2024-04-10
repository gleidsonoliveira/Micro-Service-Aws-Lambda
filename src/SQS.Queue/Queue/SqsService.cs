﻿using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using SQS.Queue.Queue.Dtos.Request;
using SQS.Queue.Queue.Dtos.Response;
using SQS.Queue.Queue.Interface;

namespace SQS.Queue.Queue
{
    public class SqsService : ISqsService
    {
        private readonly AmazonSQSClient _cliente;
        public SqsService()
        {
            _cliente = new AmazonSQSClient();
        }
        public async Task DeleteAsyncMessage(string QueueUrl, string Id)
        {
            await _cliente.DeleteMessageAsync(QueueUrl, Id);
        }

        public async Task<List<MessageTratadaResponse<T>>> FetchFileAsyncMessage<T>(MessageRequest request)
        {
            ReceiveMessageResponse mensagemResponse = await _cliente.ReceiveMessageAsync(request.FilaUrl);
            if (mensagemResponse == null)
            {
                return null;
            }

            List<MessageTratadaResponse<T>> mensagens = new List<MessageTratadaResponse<T>>();
            foreach (Message mensagem in mensagemResponse.Messages)
            {
                T mensagemDeserealizada = UnrealizeMessage<T>(mensagem);
                if (mensagemDeserealizada != null)
                {
                    mensagens.Add(new MessageTratadaResponse<T>
                    {
                        ReceiptHandle = mensagem.ReceiptHandle,
                        Message = mensagemDeserealizada,
                        MessageId = mensagem.MessageId
                    });
                }
            }

            return mensagens;
        }

        public async Task<List<MessageTratadaResponse<T>>> FetchMessageGetsAsyncDeleted<T>(MessageRequest request)
        {
            ReceiveMessageRequest requestSqs = new ReceiveMessageRequest
            {
                QueueUrl = request.FilaUrl,
                MaxNumberOfMessages = request.MaximoMensagens,
                AttributeNames = request.NomeAtributos,
                MessageAttributeNames = request.MensagemNomeAtributos,
                ReceiveRequestAttemptId = request.TentativasRequestAtendida,
                VisibilityTimeout = request.VisibilidadeTimeOut,
                WaitTimeSeconds = request.EsperaSegundos
            };

            ReceiveMessageResponse mensagemResponse = await _cliente.ReceiveMessageAsync(requestSqs);

            if (mensagemResponse == null)
                return null;

            List<MessageTratadaResponse<T>> mensagens = new List<MessageTratadaResponse<T>>();
            foreach (Message mensagem in mensagemResponse.Messages)
            {
                T mensagemDeserealizada = UnrealizeMessage<T>(mensagem);
                if (mensagemDeserealizada != null)
                {
                    mensagens.Add(new MessageTratadaResponse<T>
                    {
                        ReceiptHandle = mensagem.ReceiptHandle,
                        Message = mensagemDeserealizada
                    });
                }

                await _cliente.DeleteMessageAsync(request.FilaUrl, mensagem.ReceiptHandle);
            }

            return mensagens;
        }

        public async Task<MessageResponse> SendFileAsyncMessage<T>(string filaUrl, T request)
        {
            SendMessageResponse response = await _cliente.SendMessageAsync(filaUrl, JsonConvert.SerializeObject(request));
            if (response != null)
            {
                return new MessageResponse
                {
                    Atributos = response.MD5OfMessageAttributes,
                    AtributosSistema = response.MD5OfMessageSystemAttributes,
                    MensagemCorpo = response.MD5OfMessageBody,
                    MensagemId = response.MessageId,
                    NumeroSequencial = response.SequenceNumber
                };
            }

            return null;
        }

        public T UnrealizeMessage<T>(Message mensagem)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(mensagem.Body);
            }
            catch
            {
                return default(T);
            }
        }
    }
}