using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Model;
using AutoMapper;

namespace api.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(){
            CreateMap<User, MemberDTO>();
            CreateMap<RegisterDTO, User>();

            CreateMap<UserTokenDTO, UserTokens>();

            CreateMap<Message, MessageDTO>();
            CreateMap<MessageDTO, Message>();
        }
    }
}