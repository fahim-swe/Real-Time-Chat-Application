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
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IRabbitMQPublish _messagePublish;
        private readonly IMapper _mapper;
        private readonly IRabbitMQService _service;
        public MessageController(IMessageRepository messageRepository, 
            IMapper mapper, IRabbitMQPublish messagePublish,
            IRabbitMQService service){
            _messageRepository = messageRepository;
            _mapper = mapper;
            _messagePublish = messagePublish;
            _service = service;
        }

     
    }
}