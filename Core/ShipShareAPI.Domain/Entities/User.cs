using ShipShareAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ShipShareAPI.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfilePicture { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public List<TravellerPost>? TravellerPosts { get; set; }
        public List<SenderPost>? SenderPosts { get; set; }
        public List<Conversation>? Conversations { get; set; }
        public List<Review>? GivenReviews { get; set; }
        public List<Notification>? Notifications { get; set; }
        public List<Transaction>? CompletedTransactions { get; set; }

    }
}
