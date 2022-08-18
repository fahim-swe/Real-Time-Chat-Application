using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.DTOs;
using api.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace api.RabbitMQ
{
    public class RabbitMQPublish : IRabbitMQPublish
    {
        public Task  sendMessageToQueue(PublishMessageDto publishMessage)
        {
           var factory = new ConnectionFactory() {
                Uri = new Uri("amqps://tbkngyyq:VoupLAj0d9yyDUOXQaKkqJuH8ABMFYXT@puffin.rmq2.cloudamqp.com/tbkngyyq"),
                VirtualHost = "tbkngyyq",
                Port = 5671,
                Password = "VoupLAj0d9yyDUOXQaKkqJuH8ABMFYXT"
                };
            string databaseQueue = "DATABASEQueue";
            string queueName = "SignalRQueue";
            string exchangeName = "ExchangerQueue";

            using(var connection = factory.CreateConnection())
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