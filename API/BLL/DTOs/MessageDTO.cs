using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.DTOs
{
    public class MessageDTO
    {
        public string message { get; set; }
        public Guid ChatID { get; set; }
        public Guid UserID { get; set; }
        public DateTime DateTime { get; set; }
    }
}
