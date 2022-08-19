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
        
        private readonly IRabbitMQPublish _rabbitMQPublish;
    
        public MessageHub(IRabbitMQPublish rabbitMQPublish){
            _rabbitMQPublish = rabbitMQPublish;

        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext?.Request.Query["user"].ToString();

            var userId = Context.User.GetUserId();
            string groupName = GetGroupName(userId, otherUser);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

     

        public async Task SendMessage(MessageDTO message)
        {
            
            var userId = Context.User?.GetUserId();

            if(userId == message.RecipientId){
                throw new HubException("You cann't send message to yourself");
            }

            var publishMessage = new Message
            {   
                SenderId = userId,
                RecipientId = message.RecipientId,
                Content = message.Content,
                MessageSent = DateTime.Now,
            };
            
            await _rabbitMQPublish.sendMessageToQueue(publishMessage);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        
        private string GetGroupName(string getUserId, string? otherUser)
        {
            var stringCompare = string.CompareOrdinal(getUserId.ToString(), otherUser) < 0;
            return stringCompare ? $"{getUserId.ToString()}-{otherUser}" : $"{otherUser}-{getUserId.ToString()}";
        }

    }
}