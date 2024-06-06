using MyBasket.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBasket.Domain.Repository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        void Update(Category category);
    }
}
