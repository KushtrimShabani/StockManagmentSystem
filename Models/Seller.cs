using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektiSMS.Models
{
    public class Seller :BaseEntity
    {
        public int Id { get; set; }
        public double price_sell { get; set; }
        public int quantity { get; set; }
        public DateTime timeSeller { get; set; }

        [ForeignKey("Product")]
        public int productID { get; set; }
        public virtual Product product { get; set; }
    }
}
