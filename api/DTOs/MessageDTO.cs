using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class MessageDTO
    {
        public string RecipientId {get; set;}
        
        public string Content {get; set;}

        public DateTime MessageSent {get; set;}
    }
}