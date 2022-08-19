using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Model;

namespace api.Services
{
    public interface IMessageRepository
    {
        Task AddMessage(Message message);
        Task<List<Message>> GetMessage(string currentUserId, string recipientId);
    }
}