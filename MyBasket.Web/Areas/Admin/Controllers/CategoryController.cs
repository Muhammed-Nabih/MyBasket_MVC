using Microsoft.AspNetCore.Mvc;
using MyBasket.Domain;
using MyBasket.Domain.Models;
using MyBasket.Domain.Repository;
using MyBasket.Domain.ViewModels;
using MyBasket.Infrastructure.Implementation;


namespace MyBasket.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }

        /******************************************* Create ****************************************/

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Complete();
                TempData["Create"] = "Data Has Created Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        /******************************************* EDIT ****************************************/

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            var categoryIndb = _unitOfWork.Category.GetFirstorDefault(x => x.Id == id);
            return View(categoryIndb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Complete();
                TempData["Update"] = "Data Has Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }


        /******************************************* Delete ****************************************/

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }
            var categoryIndb = _unitOfWork.Category.GetFirstorDefault(x => x.Id == id);
            return View(categoryIndb);
        }

        [HttpPost]
        public IActionResult DeleteCategory(int? id)
        {
            var categoryIndb = _unitOfWork.Category.GetFirstorDefault(x => x.Id == id);
            if (categoryIndb == null)
            {
                NotFound();
            }
            _unitOfWork.Category.Remove(categoryIndb);
            _unitOfWork.Complete();
            TempData["Delete"] = "Data Has Deleted Successfully";
            return RedirectToAction("Index");
        }



    }
}
