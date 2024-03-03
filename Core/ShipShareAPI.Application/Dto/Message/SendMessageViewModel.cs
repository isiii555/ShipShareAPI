using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Dto.Message
{
    public class SendMessageViewModel
    {
        public Guid RecipientId { get; set; }
        public string? Text { get; set; }
    }
}
