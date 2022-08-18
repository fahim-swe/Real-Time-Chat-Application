using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.DTOs;
using api.Model;
using api.Services;
using AutoMapper;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace api.RabbitMQ
{
    public class RabbitMQDBConsumer : IDBConsumer
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly IServiceProvider _serviceProvider;
        private readonly IMessageRepository _message;
        private readonly IMapper _mapper;
        private string queueName = "DATABASEQueue";
    
        public RabbitMQDBConsumer(IServiceProvider serviceProvider, IMessageRepository message,
            IMapper mapper
        )
        {
            _factory = new ConnectionFactory() {
                Uri = new Uri("amqps://tbkngyyq:VoupLAj0d9yyDUOXQaKkqJuH8ABMFYXT@puffin.rmq2.cloudamqp.com/tbkngyyq"),
                VirtualHost = "tbkngyyq",
                Port = 5671,
                Password = "VoupLAj0d9yyDUOXQaKkqJuH8ABMFYXT"
                };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _message = message;

            _mapper = mapper;
 
            _serviceProvider = serviceProvider;
        }
 
        public  virtual  void Connect()
        {
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
 
            var consumer = new EventingBasicConsumer(_channel);
 
            consumer.Received += async delegate (object? model, BasicDeliverEventArgs ea) {
        
                byte[] body = ea.Body.ToArray();

                var data = Encoding.UTF8.GetString(body);
              
                try{
                    var result = JsonConvert.DeserializeObject<PublishMessageDto>(data);
                    Console.WriteLine(result);

                    var message = new Message
                    {   
                        SenderId = result.SenderId,
                        RecipientId = result.RecipientId,
                        Content = result.Content
                    };

                    Console.WriteLine( "DATABASE" + message);
                    await _message.AddMessage(message);
                        
                }catch(Exception e){
                    Console.WriteLine(e);
                }    
            };
            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}