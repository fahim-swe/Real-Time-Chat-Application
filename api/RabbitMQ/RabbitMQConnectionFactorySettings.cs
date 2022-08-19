using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.RabbitMQ
{
    public class RabbitMQConnectionFactorySettings
    {
        public string Uri {get; set; } = null!;
        public string VirtualHost {get; set;} = null!;
        public int Port {get; set;}
        public string Password {get; set; } = null!;
        
    }
}