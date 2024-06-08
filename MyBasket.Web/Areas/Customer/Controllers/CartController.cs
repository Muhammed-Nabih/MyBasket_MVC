using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBasket.Domain.Repository;
using MyBasket.Domain.ViewModels;
using System.Security.Claims;

namespace MyBasket.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                CartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, Includeword:"Product")
            };

            foreach(var item in ShoppingCartVM.CartList)
            {
                ShoppingCartVM.TotalCarts += (item.Count * item.Product.Price);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int cartid)
        {
            var shoppingcart = _unitOfWork.ShoppingCart.GetFirstorDefault(x=>x.Id == cartid);
            _unitOfWork.ShoppingCart.IncreaseCount(shoppingcart, 1);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

		public IActionResult Minus(int cartid)
		{
			var shoppingcart = _unitOfWork.ShoppingCart.GetFirstorDefault(x => x.Id == cartid);
            if(shoppingcart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(shoppingcart);
				_unitOfWork.Complete();
				return RedirectToAction("Index","Home");
			}
            else
            {
            _unitOfWork.ShoppingCart.DecreaseCount(shoppingcart, 1);
            }
			_unitOfWork.Complete();
			return RedirectToAction("Index");
		}


		public IActionResult Remove(int cartid)
		{
			var shoppingcart = _unitOfWork.ShoppingCart.GetFirstorDefault(x => x.Id == cartid);
			_unitOfWork.ShoppingCart.Remove(shoppingcart);
			_unitOfWork.Complete();
			return RedirectToAction("Index");
		}

	}
}
