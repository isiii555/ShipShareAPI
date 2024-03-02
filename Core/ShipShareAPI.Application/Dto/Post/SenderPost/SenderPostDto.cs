using ShipShareAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Dto.Post.SenderPost
{
    public class SenderPostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int Views { get; set; }
        public string Description { get; set; } = null!;
        public string StartDestination { get; set; } = null!;
        public string EndDestination { get; set; } = null!;
        public DateTime DeadlineDate { get; set; }
        public List<string>? ItemPhotos { get; set; }
        public string ItemType { get; set; } = null!;
        public float ItemWeight { get; set; }
    }
}
