using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektiSMS.Models
{
    public class Stock :BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price_sell { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }

        [ForeignKey("Product")]
       // public static List<Product> productID = new List<Product>();
     public int productID { get; set; }
        public virtual Product product { get; set; }
    }
}
