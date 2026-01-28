using Ecom.Core.Dto;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositries
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IEmailService emailService;
        private readonly IGenerateToken generateToken;

        public AuthRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IGenerateToken generateToken)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
            this.generateToken = generateToken;
        }


        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return null;
            }

            if (await userManager.FindByNameAsync(registerDto.UserName) != null)
            {
                return "This UserName Is Alredy Register";
            }

            if (await userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                return "This Email Is Alredy Register";

            }

            AppUser user = new AppUser()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName

            };


            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return result.Errors.ToList()[0].Description;
            }

            //Send Active Email 

            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            await SendEmail(user.Email, token, "Active", "ActiveEmail", "Active Your Email,Click Buttom To Active");
        
            return "Done";

             
        }

        public async Task SendEmail(string email, string code, string component, string subject, string message)
        {
            var result = new EmailDto(
                
                  email
                , "degodego107@gmail.com"
                , subject
                , EmailStringBody.send(email, code, component, message));
            await emailService.SendEmail(result);


        }


        public async Task<string> LoginAsync(LoginDto login)
        {
            if (login == null)
            {
                return null;
            }
            var finduser = await userManager.FindByEmailAsync(login.Email);
            
            if (!finduser.EmailConfirmed)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(finduser);
                await SendEmail(finduser.Email, token, "Active", "ActiveEmail", "Active Your Email,Click Buttom To Active");
                return "Please confirm your email first , we have sent active to your e_mail"; 
            }
          
            var result = await signInManager.CheckPasswordSignInAsync(finduser, login.  Password , true);
         
            if (result.Succeeded)
            {
                return generateToken.GetAndCreateToken(finduser);
            }
            return "Please Check Your Email Or Password,Something Went Wrong";
        }


        public async Task<bool> SendingEmailForForgetPassword(string email)
        {
          var finduser = await userManager.FindByEmailAsync(email);
            if (finduser == null)
            {
                return false;

            }
            var token = await userManager.GeneratePasswordResetTokenAsync(finduser);
            await SendEmail(finduser.Email, token, "Active", "Reset-Password", "Reset Password");
            return true;


        }

        public async Task<string> ResetPassword(ResetPasswordDto resetPassword)
        {
            var finduser = await userManager.FindByEmailAsync(resetPassword.Email);
            if (finduser == null)
            {
                return null;
            }
            var result = await userManager.ResetPasswordAsync(finduser, resetPassword.Token, resetPassword.Password);
            if (result.Succeeded)
            {
                return "Password Changed Success";
            }
            return result.Errors.ToList()[0].Description;
            

        }

        public async Task<bool> ActiveAccount(ActiveAccountDto accountDto)
        {
            var finduser = await userManager.FindByEmailAsync(accountDto.Email);
            if (finduser == null)
            {
                return false;
            }
            var result = await userManager.ConfirmEmailAsync(finduser , accountDto.Token);
            if (result.Succeeded)
            {
                return true;
            }
            var token = await userManager.GenerateEmailConfirmationTokenAsync(finduser);
            await SendEmail(finduser.Email, token, "Active", "ActiveEmail", "Active Your Email,Click Buttom To Active");
            return false;

        }


    }
}


    

