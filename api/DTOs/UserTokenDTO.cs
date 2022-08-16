using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class UserTokenDTO
    {
        public string id {get; set;}
        
        public string UserName {get; set;
        }

        public string Token {get; set;}
        public string RefreshToken {get;set;
        }
        public DateTime ExpiredTime { get;set;}
    }
}