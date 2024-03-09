using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Dto.Conversation
{
    public class ConversationNameIdDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
