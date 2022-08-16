using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
namespace api.Controllers
{
  [ApiController]
  [Route("[controller]")]
  [Authorize]
  public class OnlineController : ControllerBase
  {
  IOnlineService _onlineService;

    public OnlineController(IOnlineService onlineService){
      _onlineService = onlineService;
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> beat(string id)
    {
      Console.WriteLine(id);
      return Ok(await _onlineService.updateOnlineStatus(id));
    }

    [HttpGet("users")]
    public async Task<IActionResult> getOnlineUsers(){
    
      return Ok(await _onlineService.getOnlineUsers());
    }
    
  }
  
}