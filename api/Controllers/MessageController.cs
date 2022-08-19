using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Helper;
using api.RabbitMQ;
using api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MessageController : Controller
    {        
        private readonly IMessageRepository  _messageRepository;
        private readonly IMapper _mapper;
    
        public MessageController(IMessageRepository messageRepository, IMapper mapper, 
            IRabbitMQPublish rabbitMQPublish){
            _messageRepository = messageRepository;
            _mapper = mapper;
        }


        [HttpGet("{recipientId}")]
        public async Task<IActionResult> GetMessage(string recipientId)
        {
            var senderId = User.GetUserId();
            var messages = await _messageRepository.GetMessage(senderId, recipientId);
            return Ok(messages);
        }
    }
}