using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProdigiousJuanOsorioTest.Source.BusinessLayer;
using ProdigiousJuanOsorioTest.Source.DataLayer;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using ProdigiousJuanOsorioTest.Models;

namespace ProdigiousJuanOsorioTest.Api
{
    public class ProductController : ApiController
    {
        [HttpGet]
        [Route("prodigious/api/products")]
        [Authorize(Roles ="author,visitor")]
        public HttpResponseMessage GetAllProducts()
        {
            ProductManager pm = new ProductManager();
            var productList = pm.GetAllProducts();

            var productListDto = productList.Select(p => new ProductDto()
            {
                Id = p.ProductId,
                Name = p.Name,
                Number = p.ProductNumber,
                Price = p.Price,
                PhotoName = p.ThumbNailPhotoFileName
            });

            var responseData = JsonConvert.SerializeObject(productListDto, Formatting.None);
            return CreateResponse(responseData);
        }
        

        [HttpGet]
        [Route("prodigious/api/singleproduct")]
        [Authorize(Roles = "author,visitor")]
        public HttpResponseMessage GetProductById(int productId)
        {
            ProductManager pm = new ProductManager();
            var product = pm.GetProductById(productId);
            var responseData = "{\"message\": \"No product was found with Id: " + productId + "\"}";

            if (product != null)
            {
                var productDto = new ProductDto()
                {
                    Id = product.ProductId,
                    Name = product.Name,
                    Number = product.ProductNumber,
                    Price = product.Price,
                    PhotoName = product.ThumbNailPhotoFileName
                };

                responseData = JsonConvert.SerializeObject(productDto, Formatting.None);
            }
            
            return CreateResponse(responseData);
        }

        [HttpPost]
        [Route("prodigious/api/createproduct")]
        [Authorize(Roles = "author")]
        public HttpResponseMessage CreateProduct([FromBody]string value)
        {
            var p = JsonConvert.DeserializeObject<ProductDto>(value);
            
            ProductManager pm = new ProductManager();
            var product = new Product()
            {
                Name = p.Name,
                ProductNumber = p.Number,
                Price = p.Price,
                ThumbNailPhotoFileName = p.PhotoName
            };
            
            var data = pm.InsertProduct(product);
            var responseData = JsonConvert.SerializeObject(data, Formatting.None);
            return CreateResponse(responseData);
        }

        [HttpPut]
        [Route("prodigious/api/updateproduct")]
        [Authorize(Roles = "author")]
        public HttpResponseMessage UpdateProduct([FromBody]string value)
        {
            var p = JsonConvert.DeserializeObject<ProductDto>(value);

            ProductManager pm = new ProductManager();
            var product = new Product()
            {
                ProductId = p.Id,
                Name = p.Name,
                ProductNumber = p.Number,
                Price = p.Price,
                ThumbNailPhotoFileName = p.PhotoName
            };
            var data = pm.UpdateProduct(product);
            var responseData = JsonConvert.SerializeObject(data, Formatting.None);
            return CreateResponse(responseData);
        }

        private HttpResponseMessage CreateResponse(string responseData)
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new ObjectContent<string>(responseData, new JsonMediaTypeFormatter());
            resp.Headers.ConnectionClose = true;
            resp.Headers.CacheControl = new CacheControlHeaderValue();
            resp.Headers.CacheControl.Public = true;

            return resp;
        }

    }
}
