using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Model
{
    
    public class User
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]     
        public string id {get; set;}

        [BsonElement("UserName"), BsonRepresentation(BsonType.String)]
        public string UserName {get; set;} = null!;
        
       
        public byte[] PasswordHash {get; set;} =null!;

   
        public byte[] PasswordSalt {get; set;} =null!;

       
        public DateTime BirthDate {get; set;} 

        
        public string Email {get; set;} = null!;
    
    }
}