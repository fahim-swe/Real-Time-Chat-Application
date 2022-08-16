using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Helper;
using api.Model;
using api.RabbitMQ;
using api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace api.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        private readonly IRabitMQSignalRConsumer _rabitMQSignalRConsumer;
        private readonly IRabbitMQPublish _rabbitMQPublish;

        public MessageHub(IMessageRepository messageRepository, IMapper mapper,
            IRabitMQSignalRConsumer consumer, IRabbitMQPublish rabbitMQPublish){
            _messageRepository = messageRepository;
            _mapper = mapper;
            _rabitMQSignalRConsumer = consumer;
            _rabbitMQPublish = rabbitMQPublish;

        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext?.Request.Query["user"].ToString();

            var userId = Context.User.GetUserId();
            string groupName = GetGroupName(userId, otherUser);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var message = await _messageRepository
                .GetMessageThred(userId, otherUser);
            
            _rabitMQSignalRConsumer.setQueueName(userId + otherUser, groupName);
            await Clients.Group(groupName).SendAsync("RecieveMessageThread", message);
        }

        private string GetGroupName(string v, string? otherUser)
        {
            var stringCompare = string.CompareOrdinal(v, otherUser) < 0;
            return stringCompare ? $"{v}-{otherUser}" : $"{otherUser}-{v}";
        }

        public async Task SendMessage(MessageDTO message)
        {
            var userId = Context.User?.GetUserId();

            if(userId == message.RecipientId){
                throw new HubException("You cann't send message to yourself");
            }

            var _message = new Message
            {   
                SenderId = userId,
                RecipientId = message.RecipientId,
                Content = message.Content
            };

            _rabbitMQPublish.isOnline(true);
            var exchangeName = GetGroupName(message.RecipientId, userId);
            await _rabbitMQPublish.sendMessageToQueue(message.RecipientId + userId, exchangeName,_message);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _rabitMQSignalRConsumer.reset();
            _rabbitMQPublish.isOnline(false);
            await base.OnDisconnectedAsync(exception);
        }

        private string GetGroupName(Func<string> getUserId, string? otherUser)
        {
            var stringCompare = string.CompareOrdinal(getUserId.ToString(), otherUser) < 0;
            return stringCompare ? $"{getUserId.ToString()}-{otherUser}" : $"{otherUser}-{getUserId.ToString()}";
        }

    }
}