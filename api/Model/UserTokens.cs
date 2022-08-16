using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Model
{
    public class UserTokens
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]     
        public string id {get; set;}
        
        public string UserName {get; set;
        }
        public string RefreshToken {get;set;
        }
    
        public DateTime ExpiredTime { get;set;}
    }
}