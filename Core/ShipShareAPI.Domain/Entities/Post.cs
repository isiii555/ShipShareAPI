using ShipShareAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Domain.Entities
{
    public class Post : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Title { get; set; } = null!;
        public int Views { get; set; }
        public string Description { get; set; } = null!;
        public string StartDestination { get; set; } = null!;
        public string EndDestination { get; set; } = null!;
        public DateTime DeadlineDate { get; set; }
        public User? User { get; set; }
    }
}
