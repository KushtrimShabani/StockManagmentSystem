﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektiSMS.Models
{
    public class Product :BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price_buy { get; set; }
        public double Price_sell { get; set; }
        public int Quantity { get; set; }
        public string Measure { get; set; }
        public bool Active { get; set; }
        public string Image { get; set; }

      /*  [ForeignKey("Stock")]
        public int stockID { get; set; }
        public virtual Stock stock { get; set; }*/
    }
}
