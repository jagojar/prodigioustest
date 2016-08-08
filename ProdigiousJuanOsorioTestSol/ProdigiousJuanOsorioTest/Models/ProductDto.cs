using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProdigiousJuanOsorioTest.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public decimal Price { get; set; }        
        public string PhotoName { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}