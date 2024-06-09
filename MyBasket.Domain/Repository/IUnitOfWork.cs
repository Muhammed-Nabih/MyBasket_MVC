using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBasket.Domain.Repository
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryRepository Category { get; }
        IProductRepsitory Product { get; }
        IShoppingCartRepository ShoppingCart { get; }
		IOrderHeaderRepository OrderHeader { get; }
		IOrderDetailRepository OrderDetail { get; }
        IApplicationUserRepository ApplicationUser { get; }
        int Complete();
    }
}
