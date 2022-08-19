using api.Model;
using StackExchange.Redis;
using System.Security.Claims;
using AutoMapper;
using api.DTOs;
using System.Security.Cryptography;
namespace api.Services
{
  public class OnlineService : IOnlineService
  {
    private IDatabase _cache;
    private IServer _server;
    private IUserService _userUservice;
    private IMapper _mapper;
    

    public OnlineService(IConnectionMultiplexer connectionMultiplexer,IMapper mapper
      ,IUserService userService,IServer server)
    {
      _cache = connectionMultiplexer.GetDatabase();
      _userUservice = userService;
      _server =  server;
      _mapper = mapper;
    }
    public async Task<List<MemberDTO>> getOnlineUsers()
    {
      RedisKey[] onlineUsersToken =  _server.Keys(pattern:"look_at_baby:online:*").ToArray();
      List<MemberDTO> users = new List<MemberDTO>();
      if (onlineUsersToken != null)
      {
        foreach (string? token in onlineUsersToken)
        {
          var id = token?.Split(":")[2];
          
          
          var user = await _userUservice.getByIdAsync(id);
          users.Add(_mapper.Map<MemberDTO>(user)); 
          
        }
      }
      return users;
    }

      public async Task<bool> updateOnlineStatus(string key)
      {
      return await _cache.StringSetAsync("look_at_baby:online:"+key,"online",TimeSpan.FromMinutes(1));
      }
    }
}