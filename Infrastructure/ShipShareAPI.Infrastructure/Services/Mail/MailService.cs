using Microsoft.Extensions.Options;
using MimeKit;
using ShipShareAPI.Application.Interfaces.Services;
using ShipShareAPI.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Infrastructure.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly MailOptions _mailOptions;
        public MailService(IOptions<MailOptions> options) {
            _mailOptions = options.Value;
        }
        public async Task SendMessageAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            var newEmail = new MimeMessage();
            newEmail.From.Add(new MailboxAddress(_mailOptions.DisplayName, _mailOptions.Email));
            newEmail.To.Add(new MailboxAddress("Receiver", to));
            newEmail.Subject = subject;
            newEmail.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_mailOptions.Host, _mailOptions.Port, false);
            await smtp.AuthenticateAsync(_mailOptions.Email, _mailOptions.Password);
            await smtp.SendAsync(newEmail);
            await smtp.DisconnectAsync(true);
        }
    }
}
