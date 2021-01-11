
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.Models.Entities
{
    public class User
    {
        public Guid ID { get; set; }

        public string Username { get; set; }

        public string Email { get; set;  }

        public string PhoneNumber { get; set; }

        [JsonIgnore]
        public byte[] PasswordHash { get; set; }

        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }

        public List<UserRole> UserRoles { get; set; }
        public List<ChatUser> ChatUsers { get; set; }
    }
}
