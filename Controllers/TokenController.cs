using TDM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TDM.Interfaces;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace TDM.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly DapperContext _context;
        private readonly IUserInfoRepository _userInfoRep;
        private readonly IRefreshTokenGeneratorRepository _refreshTokenGeneratorRepository;
        public TokenController(IConfiguration config, DapperContext dapperContext, IUserInfoRepository userInfoRep, IRefreshTokenGeneratorRepository refreshTokenGeneratorRepository)
        {
            _configuration = config;
            _context = dapperContext;
            _userInfoRep = userInfoRep;
            _refreshTokenGeneratorRepository = refreshTokenGeneratorRepository;
        }

        // UserLogin
        // POST: api/Token/Authenticate
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromForm] UserLoginModel _userInfo)
        {
            TokenResponse tokenResponse = new TokenResponse();
            try
            {
                if (_userInfo.Email != null && _userInfo.Password != null)
                {
                    UserLoginModel user = await _userInfoRep.UserLogin(_userInfo);

                    if (user != null)
                    {
                        var tokenRaw = generateJWTToken(user);
                        var token = new JwtSecurityTokenHandler().WriteToken(tokenRaw);
                        tokenResponse.JWTToken = token;
                        tokenResponse.UserId = user.UserId;
                        tokenResponse.RefreshToken = await _refreshTokenGeneratorRepository.GenerateToken(user.UserId);
                        return Ok(tokenResponse);
                    }
                    else
                    {
                        return BadRequest("Invalid credentials");
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Token/UserRegister
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UserRegister([FromForm] UserRegisterModel _userInfo)
        {
            try
            {
                if (_userInfo.Email != null && _userInfo.Password != null && _userInfo.UserName != null && _userInfo.DisplayName != null)
                {
                    bool isUserNameExists = await _userInfoRep.IsUserNameExist(_userInfo.UserName);
                    if (isUserNameExists == false) {
                        UserRegisterModel user = await _userInfoRep.UserRegister(_userInfo);
                        UserLoginModel userLogin = new UserLoginModel() {
                            Email = user.Email,
                            Password = user.Password
                        };
                        return await Authenticate(userLogin);
                    }
                    else {
                        return Ok("usernameexists");    
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [NonAction]
        private JwtSecurityToken generateJWTToken(UserLoginModel obj)
        {
            //create claims details based on the user information
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", obj.UserId.ToString()),
                new Claim("DisplayName", obj.DisplayName),
                new Claim("UserName", obj.UserName),
                new Claim("Email", obj.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: GetJWTExpireTime(),
                signingCredentials: signIn
            );

            return token;
        }

        [NonAction] //UserLogin
        public async Task<TokenResponse> Authenticate(long userId, Claim[] claims)
        {
            UserLoginModel user = await _userInfoRep.GetUserDetails(userId);
            var tokenRaw = generateJWTToken(user);
            TokenResponse tokenResponse = new TokenResponse();
            tokenResponse.JWTToken = new JwtSecurityTokenHandler().WriteToken(tokenRaw);
            tokenResponse.RefreshToken = await _refreshTokenGeneratorRepository.GenerateToken(userId);
            tokenResponse.UserId = userId;

            return tokenResponse;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Refresh([FromForm] TokenResponse token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token.JWTToken);
            
            Task<string?> _reftable = _refreshTokenGeneratorRepository.GetUserRefreshToken(token.UserId, token.RefreshToken);
            if (_reftable == null)
            {
                return Unauthorized();
            }
            TokenResponse _result = await Authenticate(token.UserId, securityToken.Claims.ToArray());
            return Ok(_result);
        }

        [NonAction]
        private DateTime GetJWTExpireTime() {
            return DateTime.Now.AddMinutes(60);
        }
    }
}