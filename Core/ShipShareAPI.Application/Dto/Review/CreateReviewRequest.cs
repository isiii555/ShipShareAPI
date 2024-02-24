using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Dto.Review
{
    public class CreateReviewRequest
    {
        public int Rating { get; set; }
        public string? Text { get; set; }
        public Guid SenderId { get; set; }
        public Guid PostId { get; set; }
    }
}
