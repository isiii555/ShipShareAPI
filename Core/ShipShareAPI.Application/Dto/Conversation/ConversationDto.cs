using ShipShareAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Dto.Conversation
{
    public class ConversationDto
    {
        public Guid Id { get; set; }
        [JsonIgnore]
        public List<User> Users { get; set; } = null!;
        [JsonIgnore]
        public List<Domain.Entities.Message>? Messages {  get; set; }
    }
}
