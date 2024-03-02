using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Dto.Notification
{
    public class CreateNotificationRequest
    {
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public string? Description {  get; set; }
    }
}
