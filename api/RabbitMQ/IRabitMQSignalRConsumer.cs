using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.RabbitMQ
{
    public interface IRabitMQSignalRConsumer
    {
        void Connect();
        void setQueueName(string queue, string group);
        void reset();
    }
}