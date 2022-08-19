
using System.Text;
using api.DTOs;
using api.Model;
using api.SignalR;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
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
 
        public RabitMQSignalRConsumer(IServiceProvider serviceProvider, IMapper mapper, IOptions<RabbitMQConnectionFactorySettings> rabbitMQConnectionString)
        {

            _factory = new ConnectionFactory()
            {
                Uri = new Uri(rabbitMQConnectionString.Value.Uri),
                VirtualHost = rabbitMQConnectionString.Value.VirtualHost,
                Port = rabbitMQConnectionString.Value.Port,
                Password = rabbitMQConnectionString.Value.Password
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

                var data = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<Message>(data);

                Console.WriteLine(queueName + "Checking: " + message);

                var group = GetGroupName(message.SenderId, message.RecipientId);
                
                await chatHub.Clients.Group(group).SendAsync("NewMessage", message);
            };
 
            _channel.BasicConsume(queue: queueName ,autoAck: true, consumer: consumer);
        }


        private string GetGroupName(string getUserId, string? otherUser)
        {
            var stringCompare = string.CompareOrdinal(getUserId.ToString(), otherUser) < 0;
            return stringCompare ? $"{getUserId.ToString()}-{otherUser}" : $"{otherUser}-{getUserId.ToString()}";
        }

    }


  
}