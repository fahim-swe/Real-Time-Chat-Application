using api.DTOs;
using api.Helper;
using api.Model;
using api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController :  ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public UsersController(IUserService userService, IMapper mapper, IUriService uriService){
            _userService = userService;
            _mapper = mapper;
            _uriService = uriService;
        }



        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery]PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            
            var users = await _userService.getUserAsync(validFilter);

            var totalRecords = await _userService.CountAsync();
            
            var data = _mapper.Map<List<MemberDTO>>(users);

            var pagedResponse = PaginationHelper.CreatePagedReponse<MemberDTO>(data, validFilter, totalRecords, _uriService, route);

            return Ok(pagedResponse);
        }
        
        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var user = await _userService.getByIdAsync(id);
            if(user == null) return NotFound("User Not found");

            return Ok(new Response<MemberDTO>(_mapper.Map<MemberDTO>(user)));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(string id, RegisterDTO registerDTO)
        {
            if(!ModelState.IsValid){
                return BadRequest("Bad Input Format");
            }


            var _user = await _userService.getByIdAsync(id);
            if(_user == null) return NotFound("User Doesn't Exit");
            
            _user = await _userService.getByUserNameAsync(registerDTO.UserName);
            if(_user != null && _user.UserName != registerDTO.UserName) return Conflict("User Name already Exit");
            
            if(await _userService.isEmailExitAsync(registerDTO.Email) && _user.Email != registerDTO.Email) return Conflict("Email alread exit");
            if(!HelperFunction.IsBirthDayValid(registerDTO.BirthDate)) return BadRequest("Under 18 Not Allow");

            _user = _mapper.Map<User>(registerDTO);

            HelperFunction.PasswordConfiguration(registerDTO.Password, ref _user);

            _user.id = id;
            
            await _userService.UpdateAsync(id, _user);

            return Ok(new Response<User>(_user));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userService.getByIdAsync(id);
            if(user is null) return NotFound("Not Found");

            await _userService.DeleteAsync(id);

            return Ok("Deleted Successfully");
        }       
    }
}