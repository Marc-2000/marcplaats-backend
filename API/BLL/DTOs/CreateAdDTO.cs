using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.DTOs
{
    public class CreateAdDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PicturePath { get; set; }
        public Guid CategoryID { get; set; }
        public Decimal Price { get; set; }
        public Boolean Bidding { get; set; }
    }
}
