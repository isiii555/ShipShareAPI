using ShipShareAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Domain.Entities
{
    public class ConversationUser : BaseEntity
    {
        public Guid ConversationId { get; set; }
        public Guid UserId { get; set; }
        public Conversation Conversation { get; set; }
        public User User { get; set; }
    }
}
