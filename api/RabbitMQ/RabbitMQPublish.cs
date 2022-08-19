using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.DTOs;
using api.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace api.RabbitMQ
{
    public class RabbitMQPublish : IRabbitMQPublish
    {
        private readonly ConnectionFactory _factory;
        public RabbitMQPublish(IOptions<RabbitMQConnectionFactorySettings> rabbitMQConnectionString)
        {
            _factory = new ConnectionFactory()
            {
                Uri = new Uri(rabbitMQConnectionString.Value.Uri),
                VirtualHost = rabbitMQConnectionString.Value.VirtualHost,
                Port = rabbitMQConnectionString.Value.Port,
                Password = rabbitMQConnectionString.Value.Password
            };
        }
        public Task  sendMessageToQueue(Message publishMessage)
        {
           

            string databaseQueue = "DATABASEQueue";
            string queueName = "SignalRQueue";
            string exchangeName = "ExchangerQueue";

            using(var connection = _factory.CreateConnection())
            using(IModel channel = connection.CreateModel())
            {
              
                channel.QueueDeclare(queueName, true, false, false, null);
                
               
                channel.QueueDeclare(databaseQueue, true, false, false, null);

                channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true);

               
                channel.QueueBind(queueName, exchangeName, "durable");
                
                channel.QueueBind( databaseQueue, exchangeName, "durable");

                
                var _message = JsonConvert.SerializeObject(publishMessage);
                var body = Encoding.UTF8.GetBytes(_message);
                channel.BasicPublish(exchange: exchangeName,
                                    routingKey: "",
                                    basicProperties: null,
                                    body: body);

                Console.WriteLine(" [x] Sent {0}", publishMessage);

                // channel.ExchangeDelete(exchangeName, false);        
            }
            return Task.CompletedTask;
        }
    }
}