using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webserver.Models;
using webserver.Services;
using System;
using System.IO;
using System.Text;
using Razor.Templating.Core;

namespace webserver.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Email : ControllerBase
    {
        private readonly IMailService _mailService;
        public Email(IMailService mailService)
        {
            _mailService = mailService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
