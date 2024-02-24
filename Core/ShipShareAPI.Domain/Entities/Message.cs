using ShipShareAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Domain.Entities
{
    public class Message : BaseEntity
    {
        public Guid SenderId { get; set; }
        public Guid ConversationId { get; set; }
        public string Text { get; set; } = null!;
        public bool IsRead { get; set; }
        public User? Sender { get; set; }
    }
}
