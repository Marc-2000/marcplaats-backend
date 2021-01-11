using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Marcplaats_Backend.BLL.Models.Entities
{
    public class Advertisement
    {
        public Guid ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Category Category { get; set; }

        [Column(TypeName = "decimal(30,2)")]
        public decimal Price { get; set; }

        public bool Bidding { get; set; }

        public string PicturePath { get; set; }
    }
}
