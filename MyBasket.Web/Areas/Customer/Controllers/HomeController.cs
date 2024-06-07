using Microsoft.AspNetCore.Mvc;
using MyBasket.Domain.Repository;
using MyBasket.Domain.ViewModels;

namespace MyBasket.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            ShoppingCart obj = new ShoppingCart()
            {
                Product = _unitOfWork.Product.GetFirstorDefault(x => x.Id == id, Includeword: "Category"),
                Count = 1
            };
            
            return View(obj);
        }

    }
}
