using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProdigiousJuanOsorioTest.Source.DataLayer;

namespace ProdigiousJuanOsorioTest.Source.BusinessLayer
{
    public class ProductManager
    {
        public IEnumerable<Product> GetAllProducts()
        {
            IEnumerable<Product> productList;

            using (var db = new ProdigiousDB())
            {
                productList = db.Products.ToList();
            }

            return productList;
        }

        public Product GetProductById(int productId)
        {
            Product product = null;

            using (var db = new ProdigiousDB())
            {
                product = db.Products
                    .FirstOrDefault(p => p.ProductId == productId);
            }

            return product;
        }

        public bool InsertProduct(Product product)
        {
            int rowsInserted = 0;

            using(var db = new ProdigiousDB())
            {
                var productNew = db.Products.Create();

                productNew.Name = product.Name;
                productNew.ProductNumber = product.ProductNumber;
                productNew.Price = product.Price;
                productNew.ThumbNailPhotoFileName = product.ThumbNailPhotoFileName;
                productNew.ModifiedDate = DateTime.Now;
                db.Products.Add(productNew);
                rowsInserted = db.SaveChanges();
            }

            if (rowsInserted > 0)
                return true;
            else
                return false;

        }

        public bool UpdateProduct(Product product)
        {
            int rowsModified = 0;

            using (var db = new ProdigiousDB())
            {
                var productUpdt = db.Products.FirstOrDefault(p => p.ProductId == product.ProductId);                

                if (productUpdt != null)
                {
                    productUpdt.Name = product.Name;
                    productUpdt.ProductNumber = product.ProductNumber;
                    productUpdt.Price = product.Price;
                    productUpdt.ThumbNailPhotoFileName = product.ThumbNailPhotoFileName;
                    productUpdt.ModifiedDate = DateTime.Now;

                    db.Entry(productUpdt).State = System.Data.Entity.EntityState.Modified;
                    rowsModified = db.SaveChanges();
                }
            }

            if (rowsModified > 0)
                return true;
            else
                return false;

        }

    }
}