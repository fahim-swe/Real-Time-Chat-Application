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

        private bool f = true;

        public void isOnline(bool _f)
        {
            f = _f;
        }

        public Task  sendMessageToQueue(string queueName, string exchangeName, Message message)
        {
           var factory = new ConnectionFactory() {
                Uri = new Uri("amqps://tbkngyyq:VoupLAj0d9yyDUOXQaKkqJuH8ABMFYXT@puffin.rmq2.cloudamqp.com/tbkngyyq"),
                VirtualHost = "tbkngyyq",
                Port = 5671,
                Password = "VoupLAj0d9yyDUOXQaKkqJuH8ABMFYXT"
                };
            string databaseQueue = "DATABASE";
            using(var connection = factory.CreateConnection())
            using(IModel channel = connection.CreateModel())
            {
                
                if(f){
                    channel.QueueDeclare(queueName, true, false, false, null);
                }
               
                channel.QueueDeclare(databaseQueue, true, false, false, null);

                channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true);

                if(f){
                    channel.QueueBind(queueName, exchangeName, "durable");
                }
                channel.QueueBind( databaseQueue, exchangeName, "durable");

                
                var _message = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(_message);
                channel.BasicPublish(exchange: exchangeName,
                                    routingKey: "",
                                    basicProperties: null,
                                    body: body);

                Console.WriteLine(" [x] Sent {0}", message);

                channel.ExchangeDelete(exchangeName, false);        
            }

        

            return Task.CompletedTask;
        }

        
    }
}