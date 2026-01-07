using System;
using System.Collections.Generic;
using System.Text;

namespace StockMaster.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
