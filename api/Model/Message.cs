using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Model
{
    public class Message
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]     
        public string id {get; set;}
        public string SenderId {get; set;}
        public string RecipientId {get; set;}
        public string Content {get; set;}
        public DateTime? DateRead {get; set;}
        public DateTime MessageSent {get; set;} 
    }
}