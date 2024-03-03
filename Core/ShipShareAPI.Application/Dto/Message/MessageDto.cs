using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Dto.Message
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public Guid SenderId { get; set; }
        public Guid RecipientId { get; set; }
        public Guid ConversationId { get; set; }
        public string Text { get; set; } = null!;
        public bool IsRead { get; set; }
    }
}
