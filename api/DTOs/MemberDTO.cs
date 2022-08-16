using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class MemberDTO
    {
        public string id {get; set;}
        
        public string UserName {get; set;} = null!;
        
        public DateTime BirthDate {get; set;} 
        
        public string Email {get; set;} = null!;
    }
}