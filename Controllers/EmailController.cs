using Microsoft.AspNetCore.Mvc;
using TDM.Email;
using TDM.Interfaces;
using TDM.Models;

namespace TDM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserInfoRepository _userInfoRep;
        public EmailController(IConfiguration configuration, IUserInfoRepository userInfoRep)
        {
            _configuration = configuration;
            _userInfoRep = userInfoRep;
        }

        // POST: api/Email
        [HttpPost]
        public async Task<IActionResult> SendOTPEmail([FromForm] EmailModel emailModel)
        {
            try {
                string OTP = "";
            if (emailModel.ToEmail != null)
            {
                bool isEmailExist = await _userInfoRep.IsEmailExist(emailModel.ToEmail);
                if (isEmailExist == false) {
                    SendGridEmail sendGridEmail = new SendGridEmail(_configuration);
                    string toEmail = emailModel.ToEmail;
                    OTP = generateOTP();
                    string body = "This your OTP to register on 3D Models Manager: " + OTP;
                    var responce = await sendGridEmail.Execute(toEmail, body);
                    return Ok(OTP);
                }
                else
                {
                    return BadRequest("emailexists");
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

        [NonAction]
        private string generateOTP() {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            return r;
        }
    }
}