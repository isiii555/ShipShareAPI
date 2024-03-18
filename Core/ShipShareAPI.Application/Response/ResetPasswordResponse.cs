using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Response
{
    public class ResetPasswordResponse
    {
        public bool IsChanged { get; set; }
        public string Message { get; set; }
    }
}
