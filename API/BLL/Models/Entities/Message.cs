using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.Models.Entities
{
    public class Message
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public DateTime Time { get; set; }
        public string Text { get; set; }
        [JsonIgnore]
        public Guid ChatID { get; set; }
        [JsonIgnore]
        public Chat Chat { get; set; }
    }
}
