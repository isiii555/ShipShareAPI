using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Dto.Review
{
    public class UpdateReviewRequest
    {
        public int Rating { get; set; }
        public string? Text { get; set; }
    }
}
