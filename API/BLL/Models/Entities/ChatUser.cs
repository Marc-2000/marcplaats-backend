using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.Models.Entities
{
    public class ChatUser
    {
        public Guid UserID { get; set; }
        public User User { get; set; }
        public Guid ChatID { get; set; }
        public Chat Chat { get; set; }
    }
}
