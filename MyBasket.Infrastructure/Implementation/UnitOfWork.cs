using MyBasket.Domain;
using MyBasket.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBasket.Infrastructure.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Category { get; private set; }
        public IProductRepsitory Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }

        public UnitOfWork(ApplicationDbContext context) 
        {
            _context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepsitory(context);
            ShoppingCart = new ShoppingCartRepository(context);
			OrderHeader = new OrderHeaderRepository(context);
			OrderDetail = new OrderDetailRepository(context);
            ApplicationUser = new ApplicationUserRepository(context);
        }

        public int Complete()
        {
           return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
