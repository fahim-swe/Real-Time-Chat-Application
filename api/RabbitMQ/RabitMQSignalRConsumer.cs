using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.Model;
using api.SignalR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace api.RabbitMQ
{
    public class RabitMQSignalRConsumer : IRabitMQSignalRConsumer
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
 
        protected readonly IServiceProvider _serviceProvider;

        private string queue = "NULL";
        private string group = "NULL";
 
        public RabitMQSignalRConsumer(IServiceProvider serviceProvider)
        {

            _factory = new ConnectionFactory() {
                Uri = new Uri("amqps://tbkngyyq:VoupLAj0d9yyDUOXQaKkqJuH8ABMFYXT@puffin.rmq2.cloudamqp.com/tbkngyyq"),
                VirtualHost = "tbkngyyq",
                Port = 5671,
                Password = "VoupLAj0d9yyDUOXQaKkqJuH8ABMFYXT"
                };


            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
 
            _serviceProvider = serviceProvider;
        }
 
        public  virtual  void Connect()
        {            
            _channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false);
 
            var consumer = new EventingBasicConsumer(_channel);
 
            consumer.Received += delegate (object? model, BasicDeliverEventArgs ea) {
               
                var chatHub = (IHubContext<MessageHub>)_serviceProvider.GetService(typeof(IHubContext<MessageHub>));
                
                byte[] body = ea.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);
                var result = JsonConvert.DeserializeObject<Message>(message);

                Console.WriteLine(queue + "Checking: " + message);

                chatHub.Clients.Group(group).SendAsync("NewMessage", result);
            };
 
            _channel.BasicConsume(queue: queue ,autoAck: true, consumer: consumer);
        }

        public void reset()
        {
            this.queue = "NULL";
            this.group = "NULL";
            this.Connect();
        }

        public void setQueueName(string queue, string group)
        {
           this.queue = queue;
           this.group = group;
           
           this.Connect();
        }
    }
}