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
        public string? ConnectionId { get; set; }
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public string? PasswordResetToken { get; set;}
        public DateTime? RefreshTokenExpireDate { get; set; }
        public string? ProfilePicture { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public List<Role>? Roles { get; set; }
        public List<TravellerPost>? TravellerPosts { get; set; }
        public List<SenderPost>? SenderPosts { get; set; }
        public IEnumerable<Conversation>? Conversations { get; set; }
        public List<Review>? GivenReviews { get; set; }
        public List<Review>? ReceivedReviews { get; set; }
        public List<Notification>? Notifications { get; set; }

    }
}
