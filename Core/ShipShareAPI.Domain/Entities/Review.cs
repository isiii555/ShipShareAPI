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
        public Guid SenderId { get; set; }
        public int Rating { get; set; }
        public Guid PostId { get; set; }
        public string? Text { get; set; }
        public Post? Post { get; set; }
        public User? ReviewSender { get; set; }
        public bool IsConfirmed {  get; set; }
        public bool IsDeclined { get; set; }

    }
}