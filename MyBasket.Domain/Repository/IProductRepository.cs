using MyBasket.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBasket.Domain.Repository
{
    public interface IProductRepsitory : IGenericRepository<Product>
    {
        void Update(Product product);
    }
}
