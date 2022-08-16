using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.DTOs;
using api.Model;
using api.Services;
using api.SignalR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;




namespace api.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly IServiceProvider _serviceProvider;
        private readonly IMessageRepository _message;
        private string queueName = "DATABASE";
        public RabbitMQService(IServiceProvider serviceProvider, IMessageRepository message
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
 
            _serviceProvider = serviceProvider;
        }
 
        public  virtual  void Connect()
        {
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
 
            var consumer = new EventingBasicConsumer(_channel);
 
            consumer.Received += async delegate (object? model, BasicDeliverEventArgs ea) {
        
                byte[] body = ea.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);
              
                try{
                    var result = JsonConvert.DeserializeObject<Message>(message);
                    await _message.AddMessage(result);
                        
                }catch(Exception e){
                    Console.WriteLine(e);
                }    
            };
            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}