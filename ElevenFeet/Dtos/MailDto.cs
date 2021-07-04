using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenFeet.Dtos
{
    public class MailDto
    {
        public string ToMail => "info@elevenfeet.org";
        public string FromMail { get; set; }
        public string NameSurname { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}
