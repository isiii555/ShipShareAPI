using ShipShareAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Domain.Entities
{
    public class Review : BaseEntity
    {
        public Guid ReviewSenderId { get; set; }
        public Guid ReviewRecipientId { get; set; }
        public int Rating { get; set; }
        public string? Text { get; set; }
        public User? ReviewSender { get; set; }
        public User? ReviewRecipient { get; set; }
        public bool IsConfirmed {  get; set; }
        public bool IsDeclined { get; set; }

    }
}