using ShipShareAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Domain.Entities
{
    public class RoleUser : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid RoleId {  get; set; }
        public Role Role {  get; set; }
        public User User { get; set; }
    }
}
