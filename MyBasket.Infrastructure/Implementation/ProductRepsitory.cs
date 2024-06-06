using Microsoft.EntityFrameworkCore;
using MyBasket.Domain;
using MyBasket.Domain.Models;
using MyBasket.Domain.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBasket.Infrastructure.Implementation
{
    public class ProductRepsitory : GenericRepository<Product>, IProductRepsitory
    {
        private readonly ApplicationDbContext _context;

        public ProductRepsitory(ApplicationDbContext context) : base (context) 
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var ProductInDb = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if (ProductInDb != null)
            {
                ProductInDb.Name = product.Name;
                ProductInDb.Description = product.Description;
                ProductInDb.Img = product.Img;
                ProductInDb.Price = product.Price;
                ProductInDb.CategoryId = product.CategoryId;

            }
         
        }
    }
}
