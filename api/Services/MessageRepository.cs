using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Database;
using api.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api.Services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<Message> _userMessage;

        public MessageRepository(IOptions<ApiDataBaseSetttings> userNameStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                userNameStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                userNameStoreDatabaseSettings.Value.DatabaseName);

            _userMessage = mongoDatabase.GetCollection<Message>(
                userNameStoreDatabaseSettings.Value.UserMessageCollection);
        }

        public async Task AddMessage(Message message)
        {
            await _userMessage.InsertOneAsync(message);
        }

        public async Task<List<Message>> GetMessage(string currentUserId, string recipientId)
        {
            return await _userMessage.Find( x => (x.SenderId == currentUserId && x.RecipientId == recipientId) 
                                            || (x.SenderId == recipientId && x.RecipientId == currentUserId))
                                        .SortBy( x=> x.MessageSent)
                                        .ToListAsync();
        }
    }
}