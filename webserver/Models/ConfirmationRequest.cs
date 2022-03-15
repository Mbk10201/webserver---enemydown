using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Razor.Templating.Core;

namespace webserver.Models
{
    public class ConfirmationRequest
    {
        public readonly IMailService _mailService;
        public ConfirmationRequest(IMailService mailService)
        {
            _mailService = mailService;
        }

        public async Task SendConfirmation(MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
