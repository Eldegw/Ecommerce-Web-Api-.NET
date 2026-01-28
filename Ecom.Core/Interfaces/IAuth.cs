using Ecom.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto login);
        Task<bool> SendingEmailForForgetPassword(string email);
        Task<string> ResetPassword(ResetPasswordDto resetPassword);
        Task<bool> ActiveAccount(ActiveAccountDto accountDto);
    }
}
