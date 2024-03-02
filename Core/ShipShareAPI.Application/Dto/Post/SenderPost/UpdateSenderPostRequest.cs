using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Dto.Post.SenderPost
{
    public class UpdateSenderPostRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price {  get; set; }
        public string StartDestination { get; set; } = null!;
        public string EndDestination { get; set; } = null!;
        public DateTime DeadlineDate { get; set; }
        public List<IFormFile>? ItemPhotos { get; set; }
        public string ItemType { get; set; } = null!;
        public float ItemWeight { get; set; }
    }
}
