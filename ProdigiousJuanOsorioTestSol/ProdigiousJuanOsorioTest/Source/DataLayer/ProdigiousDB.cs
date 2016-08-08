using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ProdigiousJuanOsorioTest.Source.DataLayer;

namespace ProdigiousJuanOsorioTest.Source.DataLayer
{
    public class ProdigiousDB : DbContext
    {
        public ProdigiousDB() 
            : base("name=ProdigiousBDConnection")
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}