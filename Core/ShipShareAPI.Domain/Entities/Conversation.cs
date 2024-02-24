using ShipShareAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Domain.Entities
{
    public class Conversation : BaseEntity
    {
        public List<User> Users { get; set; } = null!;
        public List<Message>? Messages { get; set; }
    }
}
