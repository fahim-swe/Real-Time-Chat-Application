using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class TokenDTO
    {
        [Required] public string Token {get; set;}
        [Required] public string RefreshToken {get;set;}

        [Required] public DateTime ExpiredTime { get;set;} = DateTime.Now;
    }

}
