using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Cost { get; set; }

        public bool HasTax { get; set; }

        public DateTime CreatedDate { get; set; }

        public string State { get; set; }
    }
}
