using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using api.DTOs;
using api.Model;
using api.Services;
using api.Helper;
using AutoMapper;
namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
            private readonly IAccountService _service;
            private readonly ITokenService _tokenService;
            private readonly IMapper _mapper;
        
            
            public AccountController(IAccountService service, ITokenService tokenService, IMapper mapper){
                _service = service;
                _tokenService = tokenService;
                _mapper = mapper;

            }

            [HttpPost("register")]
            public async Task<IActionResult> CreateAccount(RegisterDTO registerDTO)
            {
                
                if(!ModelState.IsValid){
                    return BadRequest("Enter Wrong Data Format");
                }

                int age = DateTime.Now.Year - registerDTO.BirthDate.Year;
                if(age < 18) return BadRequest("age < 18 Not allow");

                var _user = await _service.GetByUserNameAsync(registerDTO.UserName);
                if(_user != null) return Conflict("username");


                if(await _service.isEmailExitAsync(registerDTO.Email)){
                    return Conflict("email");
                }

                using var hmac = new HMACSHA512();
                var user = new User{
                    UserName = registerDTO.UserName,
                    Email = registerDTO.Email, 
                    BirthDate = registerDTO.BirthDate,
                    // just store Hash of password is very danger.. 
                    // https://auth0.com/blog/adding-salt-to-hashing-a-better-way-to-store-passwords/
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                    PasswordSalt = hmac.Key
                };


                await _service.AddUserAsync(user);

                var userTokenDTO = _tokenService.CreateToken(user);
                var userToken = _mapper.Map<UserTokens>(userTokenDTO);
                await _service.AddUserTokenAsync(userToken);

                userTokenDTO.id = userToken.id;

                return Ok(new Response<UserTokenDTO>(userTokenDTO));
            }


            [HttpPost]
            [Route("login")]
            public async Task<ActionResult> LoginUser(LoginDTO loginDto )
            {
                if(!ModelState.IsValid){
                    return BadRequest("Enter wrong input Formate");
                }

                var user = await _service.GetByUserNameAsync(loginDto.UserName);
                if(user == null) return Ok ("Invalid username");


                using var hmac = new HMACSHA512(user.PasswordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

                for(int i = 0; i < computedHash.Length; i++){
                    if(computedHash[i] != user.PasswordHash[i]){
                        return BadRequest("Wrong Password");
                    }
                }


                var userTokenDTO = _tokenService.CreateToken(user);
                var userToken = _mapper.Map<UserTokens>(userTokenDTO);
                await _service.AddUserTokenAsync(userToken);
                userTokenDTO.id = user.id;

                return Ok(new Response<UserTokenDTO>(userTokenDTO));
            }   


            [HttpPost("refresh-token")]
            public async Task<IActionResult> RefreshToken(TokenDTO tokenModel)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid client request");
                }
                

                string refreshToken = tokenModel.RefreshToken;

                var principal = _tokenService.GetPrincipalFromExpiredToken(tokenModel.Token);

                Console.WriteLine(await _service.isValidRefershKey(tokenModel.RefreshToken));

                if (principal == null || !await _service.isValidRefershKey(tokenModel.RefreshToken))
                {
                    return BadRequest("Invalid access token or refresh token");
                }

                string username = principal.Identity.Name;

                var userTokenDTO = _tokenService.CreateToken(principal.Claims.ToList(), username);

                var userToken = _mapper.Map<UserTokens>(userTokenDTO);
                await _service.AddUserTokenAsync(userToken);
                userTokenDTO.id = userToken.id;
               

                return Ok(new Response<UserTokenDTO>(userTokenDTO));
        }
    }}

