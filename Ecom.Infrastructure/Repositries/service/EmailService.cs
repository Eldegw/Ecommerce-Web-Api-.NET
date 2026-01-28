using Ecom.Core.Dto;
using Ecom.Core.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositries.service
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmail(EmailDto emailDto)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Ecom", configuration["EmailSetting:From"]));
            message.Subject = emailDto.Subject;
            message.To.Add(new MailboxAddress(emailDto.To, emailDto.To));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
               Text = emailDto.Content
            };
             
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(configuration["EmailSetting:Smpt"],
                    int.Parse(configuration["EmailSetting:Port"]), true);
                    await smtp.AuthenticateAsync(configuration["EmailSetting:Username"],
                    configuration["EmailSetting:Password"]);
                    await smtp.SendAsync(message);

                }
                catch (Exception ex)
                {
                    

                }
                finally
                {
                    smtp.Disconnect(true);
                    smtp.Dispose();

                }
            }
        }
    }
}
