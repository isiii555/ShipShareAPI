using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Dto.Post.TravellerPost
{
    public class TravellerPostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public double Price { get; set; }
        public string Description { get; set; } = null!;
        public string StartDestination { get; set; } = null!;
        public string EndDestination { get; set; } = null!;
        public DateTime DeadlineDate { get; set; }

    }
}
