
using System.Text;
using api.DTOs;
using api.Model;
using api.SignalR;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace api.RabbitMQ
{
    public class RabitMQSignalRConsumer : ISignalRConsumer
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private string queueName = "SignalRQueue";
 
        public RabitMQSignalRConsumer(IServiceProvider serviceProvider, IMapper mapper)
        {

            _factory = new ConnectionFactory() {
                Uri = new Uri("amqps://tbkngyyq:VoupLAj0d9yyDUOXQaKkqJuH8ABMFYXT@puffin.rmq2.cloudamqp.com/tbkngyyq"),
                VirtualHost = "tbkngyyq",
                Port = 5671,
                Password = "VoupLAj0d9yyDUOXQaKkqJuH8ABMFYXT"
                };


            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _mapper = mapper;
 
            _serviceProvider = serviceProvider;
        }
 
        public  virtual  void Connect()
        {            
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
 
            var consumer = new EventingBasicConsumer(_channel);
 
            consumer.Received += async delegate (object? model, BasicDeliverEventArgs ea) {
               
                var chatHub = (IHubContext<MessageHub>)_serviceProvider.GetService(typeof(IHubContext<MessageHub>));
                
                byte[] body = ea.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);
                var result = JsonConvert.DeserializeObject<PublishMessageDto>(message);

                Console.WriteLine(queueName + "Checking: " + message);

                chatHub.Clients.Group(result.GroupName).SendAsync("NewMessage", _mapper.Map<Message>(result));
            };
 
            _channel.BasicConsume(queue: queueName ,autoAck: true, consumer: consumer);
        }

    }
}