using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.Models.Entities
{
    public class UserRole
    {
        public Guid UserID { get; set; }
        public User User { get; set; }
        public Guid RoleID { get; set; }
        public Role Role { get; set; }
    }
}
