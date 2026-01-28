using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.Dto;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Ecom.Api.Controllers
{

    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await work.auth.RegisterAsync(registerDto);
          
            if (result != "Done")
            {
                return BadRequest(new ResponseApi(400, result));
            }
            return Ok(new ResponseApi(200 , result));

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await work.auth.LoginAsync(loginDto);
            if (result.StartsWith("Please"))
            {
                return BadRequest(new ResponseApi(400 , result));
            }

            Response.Cookies.Append("token", result, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Domain = "localhost",
                Expires = DateTime.Now.AddDays(1),
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
            });

            return Ok(new ResponseApi(200));
        }

        [HttpPost("Active-Email")]
        public async Task<IActionResult> active(ActiveAccountDto accountDto)
        {
            var result = await work.auth.ActiveAccount(accountDto);
            return result ? Ok(new ResponseApi(200)) : BadRequest(new ResponseApi(400));

        }


        [HttpGet("Send-Email-Forget-Password")]
        public async Task<IActionResult> forget(string email)
        {
            var result = await work.auth.SendingEmailForForgetPassword(email);
            return result ? Ok(new ResponseApi(200)) : BadRequest(new ResponseApi(400));

        }


    }
}
