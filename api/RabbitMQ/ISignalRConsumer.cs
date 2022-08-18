using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.RabbitMQ
{
    public interface ISignalRConsumer
    {
        void Connect();
    }
}