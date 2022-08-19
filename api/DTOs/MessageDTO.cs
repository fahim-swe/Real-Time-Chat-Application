using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class MessageDTO
    {
        [Required]
        public string RecipientId {get; set;}
        
        [Required]
        public string Content {get; set;}

        public DateTime MessageSent {get; protected set;} = DateTime.Now;
    }
}