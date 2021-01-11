using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.Models.Entities
{
    public class Chat
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public List<Message> Messages { get; set; }
        public List<ChatUser> ChatUsers { get; set; }
    }
}
