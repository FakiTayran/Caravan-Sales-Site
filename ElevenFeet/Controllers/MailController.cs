using ElevenFeet.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ElevenFeet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        [HttpPost("MailSender")]
        public IActionResult MailSender(MailDto mailDto)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(mailDto.FromMail);
                mail.To.Add(mailDto.ToMail);
                mail.Subject = mailDto.NameSurname;
                mail.Body = mailDto.Message;

                SmtpClient message = new SmtpClient();
                message.Credentials = new NetworkCredential(mailDto.ToMail, "!Rs2232318?");
                message.Port = 587;
                message.Host = "ni-trio-win.guzelhosting.com";
                message.EnableSsl = false;
                message.Send(mail);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
