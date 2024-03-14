using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Infrastructure.Options
{
    public class MailOptions
    {
        public string Host { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName {  get; set; }
        public int Port { get; set; }
    }
}
