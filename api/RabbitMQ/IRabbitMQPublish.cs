using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Model;

namespace api.RabbitMQ
{
    public interface IRabbitMQPublish
    {
        Task sendMessageToQueue(Message publishMessage);
    }
}